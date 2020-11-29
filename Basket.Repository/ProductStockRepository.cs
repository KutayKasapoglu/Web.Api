using Basket.Dto.Dto;
using Basket.Entity.Entity;
using Basket.Repository;
using Basket.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

public class ProductStockRepository : GenericRepository<ProductStock>, IProductStockRepository
{
    private readonly ILogger<ProductStockRepository> _logger;

    public ProductStockRepository(IUnitOfWork uow, ILogger<ProductStockRepository> logger) : base(uow) {
        _logger = logger;
    }

    public async Task<ProductDto> GetProductForCityId(int productId, int cityId, int quantity)
    {
        // Aranan şehirde siparişe uygun ve/veya Min-Max Order uantity aralığında ürün olup olmaması kontrol ediliyor
        
        var entity = await GetQuery(p => p.ProductId == productId && p.Stock >= quantity && p.CityId == cityId)
            .Include(p => p.Product).Where(p => p.Product.MaxOrderQuantity >= quantity && p.Product.MinOrderQuantity <= quantity)
            .FirstOrDefaultAsync();

        if (entity != null)
        {
            return await Task.FromResult(new ProductDto()
            {
                Id = entity.ProductId,
                Price = entity.Product.Price,
                MinOrderQuantity = entity.Product.MinOrderQuantity,
                MaxOrderQuantity = entity.Product.MaxOrderQuantity,
                Stock = new StockDto()
                {
                    Id = entity.Id,
                    Quantity = entity.Stock
                }
            });
        }

        return await Task.FromResult(new ProductDto());
    }

    public void UpdateStock(int quantityToReduce, int stockId)
    {
        // Sepete başarı ile eklenen ürün ile birlikte aynı Transaction içerisinde Stock güncellemesi yapılıyor

        _logger.LogInformation($"UpdateStockRepository/StockGüncellemesi StockId:{ stockId} Adet: { quantityToReduce}");

        var entity = GetQuery(p => p.Id == stockId).FirstOrDefault();
        entity.Stock -= quantityToReduce;
        dbSet.Update(entity);
    }
}
