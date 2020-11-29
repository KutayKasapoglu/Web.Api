using Basket.Common.Enums;
using Basket.Dto;
using Basket.Dto.Dto;
using Basket.Repository.Contracts;
using Basket.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Basket.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseDto<UserDto, UserReturnTypes>> CheckUser(string userName, string password)
        {
            return await _userRepository.CheckUser(userName, password); ;
        }

    }
}
