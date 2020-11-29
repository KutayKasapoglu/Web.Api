using Basket.Common.Enums;
using Basket.Common.Helpers;
using Basket.Dto;
using Basket.Dto.Dto;
using Basket.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Repository
{
    public class BasketRepository : GenericRepository<Entity.Entity.Basket>, IBasketRepository
    {
        private IUnitOfWork unitOfWork;
        private readonly ILogger<BasketRepository> _logger;

        public BasketRepository(IUnitOfWork uow, ILogger<BasketRepository> logger) : base(uow)
        {
            unitOfWork = uow;
            _logger = logger;
        }

        public async Task<int> GetOrCreateBasketId(Guid userId)
        {
            // Kullanıcının aktif bir Basket'i yoksa, Sepete Ürün Ekleme aşamasında yeni bir Basket oluşturulmaktadır

            var basketId = new int();
            var entity = await GetQuery(p => p.UserId == userId && p.IsActive).FirstOrDefaultAsync();

            if (entity != null)
            {
                basketId = entity.Id;
            }
            else
            {
                // Yeni Basket oluşturma işlemi burada gerçekleştirilmektedir 

                var newBasket = new Entity.Entity.Basket()
                {
                    UserId = userId,
                    IsActive = true,

                };
                await dbSet.AddAsync(newBasket);
                await unitOfWork.SaveChangesAsync();
                basketId = newBasket.Id;

                _logger.LogInformation($"GetOrCreateBasketIdRepository/Yeni Sepet Oluşturuldu UserId:{ userId}" + " BasketId: " + basketId);

            }

            return await Task.FromResult(basketId);
        }

        public async Task<ResponseDto<BasketDto, GetBasketReturnTypes>> GetBasket(Guid userId)
        {
            // Aktif Sepet burada çekilmektedir

            var entity = GetQuery(p => p.UserId == userId && p.IsActive)
                .Include(p => p.User)
                .Include(p => p.BasketProducts)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.Currency).AsQueryable();

            if (entity.Any())
            {
                var response = await entity.Select(p => new BasketDto
                {
                    Id = p.Id,
                    IsActive = p.IsActive,
                    CurrencyName = EnumHelper.GetEnumNameForValue<CurrencyTypeEnums>((int)CurrencyTypeEnums.TRY),
                    User = new UserDto()
                    {
                        Id = p.User.Id,
                        GivenName = p.User.GivenName,
                        FamilyName = p.User.FamilyName
                    },
                    Products = p.BasketProducts.Select(b => new BasketProductDto()
                    {
                        Id = b.Id,
                        ProductId = b.ProductId,
                        BasketId = b.BasketId,
                        ProductName = b.Product.Name,
                        Quantity = b.Quantity,
                        CurrencyName = b.Product.Currency.Name,
                        CurrencyId = b.Product.Currency.Id,
                        Price = b.Product.Price
                    }).ToList(),
                }).FirstOrDefaultAsync();

                foreach (var product in response.Products)
                {
                    response.TotalPrice += await ConvertCurrency(product);
                }

                // Currency Basket tamamlanana kadar EUR & USD bazında değişebileceğinden anlık olarak çekilmektedir
                // Satın alma işlemi yapıldıktan sonra iptal-iade sürecinde olası hataları engellemek adına günün döviz kuru...
                // ... BasketProduct tablosunda CurrencyRate alanına yazılacaktır

                return new ResponseDto<BasketDto, GetBasketReturnTypes>()
                {
                    IsSuccess = true,
                    ResponseCode = response.Products.Count > 0 ? GetBasketReturnTypes.Success : GetBasketReturnTypes.Wrn_EmptyBasket, // Sepet'in durumuna göre kullanıcı bilgilendiriliyor
                    Data = response
                };

                // Async metoda inline olarak ulaşılmadığı için Currency bu yöntemle dönüştürüldü
                async Task<decimal> ConvertCurrency(BasketProductDto t)
                {
                    return t.Price * t.Quantity *
                        (t.CurrencyId == (int)CurrencyTypeEnums.TRY ? 1
                        : await CurrencyHelper.GetCurrencyRate(t.CurrencyName));
                }

            }

            // Sepet bulunamadı hatası için burası çalışıyor

            _logger.LogWarning($"GetOrCreateBasketIdRepository/{ EnumHelper.GetEnumDescriptionForValue(GetBasketReturnTypes.Err_BasketNotFound) } - UserId: { userId}");

            return await Task.FromResult(new ResponseDto<BasketDto, GetBasketReturnTypes>()
            {
                IsSuccess = false,
                ResponseCode = GetBasketReturnTypes.Err_BasketNotFound,
                ResponseMessage = EnumHelper.GetEnumDescriptionForValue(GetBasketReturnTypes.Err_BasketNotFound),
                Data = null
            });
        }
    }
}
