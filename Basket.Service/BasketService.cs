using Basket.Common.Enums;
using Basket.Common.Helpers;
using Basket.Dto;
using Basket.Dto.Dto;
using Basket.Dto.RequestDto;
using Basket.Entity.Entity;
using Basket.Repository.Contracts;
using Basket.Service.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Basket.Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketProductRepository _basketProductRepository;
        private readonly IBasketRepository _basketRepository;
        private readonly IProductStockRepository _productStockRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BasketService> _logger;

        public BasketService(IBasketProductRepository basketProductRepository, IBasketRepository basketRepository, IProductStockRepository productStockRepository, IUnitOfWork unitOfWork, ILogger<BasketService> logger)
        {
            _basketProductRepository = basketProductRepository;
            _basketRepository = basketRepository;
            _productStockRepository = productStockRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDto<bool, AddToBasketReturnTypes>> AddToBasket(Guid userId, AddToBasketRequestDto model)
        {
            bool isSuccess = false;
            var responseCode = new AddToBasketReturnTypes();
            var responseMessage = string.Empty;

            var product = await _productStockRepository.GetProductForCityId(model.ProductId, model.CityId, model.Quantity);

            if (product == null || product.Stock == null)
            {
                // Şehir bazlı Stock yapısı kullanıldı, bu nedenle Stock kontrolünde CityId de istenmektedir.
                // CityId için 1-5 aralığı kullanılabilir (5 için Stock tanıtılmadı / 4 için Stock'lar 0 olarak tanıtıldı)
                // Stock kontrolünde ayrıca Min-Max Order Quantity bilgileri de girdi olarak alınmaktadır

                responseCode = AddToBasketReturnTypes.Err_ProductNotFound;
                responseMessage = EnumHelper.GetEnumDescriptionForValue(AddToBasketReturnTypes.Err_ProductNotFound);
                _logger.LogWarning($" - AddToBasketService/Stock Hatası :{ JsonConvert.SerializeObject(model)}" + " UserId: " + userId);
            }
            else
            {
                var basketId = new int();
                try
                {
                    // Ürünün ekleneceği BasketId çekilmektedir

                    basketId = await _basketRepository.GetOrCreateBasketId(userId);
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    responseCode = AddToBasketReturnTypes.Err_UnexpedtedError;
                    responseMessage = e.Message + " " + (e.InnerException != null ? e.InnerException.Message : "");
                    _logger.LogError(e, $" - AddToBasketService - Model: { JsonConvert.SerializeObject(model)}" + " UserId: " + userId);
                }

                if (basketId > 0)
                {
                    using (var transaction = _basketProductRepository.BeginTransaction())
                    {
                        // Çoklu tablo güncellemesi yapıldığı için UnitOfWork ile transaction açılarak gerçekleştirilmektedir

                        try
                        {
                            var requestModel = new BasketProduct()
                            {
                                BasketId = basketId,
                                ProductId = product.Id,
                                Quantity = model.Quantity
                            };

                            _basketProductRepository.AddToBasket(requestModel);
                            _productStockRepository.UpdateStock(model.Quantity, product.Stock.Id);

                            await _unitOfWork.SaveChangesAsync();
                            transaction.Commit();

                            isSuccess = true;
                            responseCode = AddToBasketReturnTypes.Success;
                            responseMessage = EnumHelper.GetEnumDescriptionForValue(AddToBasketReturnTypes.Success);

                            _logger.LogInformation($"AddToBasketService/Başarılı Ürün Ekleme - Model: { JsonConvert.SerializeObject(model)}" + " UserId: " + userId);
                        }
                        catch (Exception e)
                        {
                            // Olası bir hata durumunda database'de yapılan değişiklikler rollback yapılarak veri kaybı yaşanması engelleniyor
                            // Gerçekleşen exceptipn loglanarak kayıt altına alınıyor

                            transaction.Rollback();

                            isSuccess = false;
                            responseCode = AddToBasketReturnTypes.Err_UnexpedtedError;
                            responseMessage = e.Message + " " + (e.InnerException != null ? e.InnerException.Message : "");

                            _logger.LogError(e, $" - GetBasketService/Transaction - Model: { JsonConvert.SerializeObject(model)}" + " UserId: " + userId);
                        }
                    }
                }
            }

            return await Task.FromResult(new ResponseDto<bool, AddToBasketReturnTypes>()
            {
                ResponseCode = responseCode,
                ResponseMessage = responseMessage,
                IsSuccess = isSuccess,
                Data = isSuccess
            });
        }

        public async Task<ResponseDto<BasketDto, GetBasketReturnTypes>> GetBasket(Guid userId)
        {
            try
            {
                // JWT ile gerçekleştirilen Authorization'dan sonra burada bir exception yakalanması öngörülmemektedir / double-check olarak eklenmiştir

                return await _basketRepository.GetBasket(userId); ;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, " - GetBasket - UserId: " + userId);
                return await Task.FromResult(new ResponseDto<BasketDto, GetBasketReturnTypes>()
                {
                    IsSuccess = false,
                    ResponseCode = GetBasketReturnTypes.Err_UnexpedtedError,
                    ResponseMessage = e.Message + " " + (e.InnerException != null ? e.InnerException.Message : "")
                });
            }
        }
    }
}
