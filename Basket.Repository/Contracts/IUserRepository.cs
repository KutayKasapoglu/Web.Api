using Basket.Common.Enums;
using Basket.Dto;
using Basket.Dto.Dto;
using Basket.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Repository.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<ResponseDto<UserDto, UserReturnTypes>> CheckUser(string userName, string password);
    }
}
