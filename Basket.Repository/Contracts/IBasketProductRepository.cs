using Basket.Dto.Dto;
using Basket.Entity.Entity;
using System.Threading.Tasks;

namespace Basket.Repository.Contracts
{
    public interface IBasketProductRepository : IRepository<BasketProduct>
    {
        void AddToBasket(BasketProduct model);
    }
}
