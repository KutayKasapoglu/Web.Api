using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Basket.Dto;
using Basket.Dto.Dto;
using Basket.Dto.RequestDto;
using Basket.Common.Enums;

namespace Basket.Service.Contracts
{
    public interface IBasketService
    {
        Task<ResponseDto<bool, AddToBasketReturnTypes>> AddToBasket(Guid userId, AddToBasketRequestDto model);
        Task<ResponseDto<BasketDto, GetBasketReturnTypes>> GetBasket(Guid userId);
    }
}
