using Basket.Dto.Dto;
using Basket.Entity.Entity;
using System.Threading.Tasks;

namespace Basket.Repository.Contracts
{
    public interface IProductStockRepository : IRepository<ProductStock>
    {
        Task<ProductDto> GetProductForCityId(int productId, int cityId, int quantity);
        void UpdateStock(int quantityToReduce, int stockId);
    }
}
