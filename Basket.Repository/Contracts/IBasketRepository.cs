using Basket.Common.Enums;
using Basket.Dto;
using Basket.Dto.Dto;
using System;
using System.Threading.Tasks;

namespace Basket.Repository.Contracts
{
    public interface IBasketRepository : IRepository<Entity.Entity.Basket>
    {
        Task<int> GetOrCreateBasketId(Guid userId);
        Task<ResponseDto<BasketDto, GetBasketReturnTypes>> GetBasket(Guid userId);
    }
}
