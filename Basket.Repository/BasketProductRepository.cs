using Basket.Dto.Dto;
using Basket.Entity.Entity;
using Basket.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Repository
{
    public class BasketProductRepository : GenericRepository<BasketProduct>, IBasketProductRepository
    {
        public BasketProductRepository(IUnitOfWork uow) : base(uow) { }

        public void AddToBasket(BasketProduct entity)
        {

            var currentBasketProduct = GetQuery(p => p.ProductId == entity.ProductId && p.BasketId == entity.BasketId).FirstOrDefault();
            
            if (currentBasketProduct != null)
            {
                // Aynı product sepette varsa yeni kayıt atmak yerine eski ürün adedini güncelliyor

                currentBasketProduct.Quantity += entity.Quantity;
                dbSet.Update(currentBasketProduct);
            }
            else
            {
                // Ürün sepette yoksa yeni bir BasketProduct kaydı oluşturuluyor

                dbSet.Add(entity);
            }
        }
    }
}
