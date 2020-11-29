using Basket.Common.Enums;
using Basket.Dto;
using Basket.Dto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Service.Contracts
{
    public interface IUserService
    {
        Task<ResponseDto<UserDto, UserReturnTypes>> CheckUser(string userName, string password);
    }
}
