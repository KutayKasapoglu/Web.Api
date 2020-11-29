using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basket.Repository.Contracts;
using Basket.Entity.Entity;
using Microsoft.EntityFrameworkCore;
using Basket.Dto.Dto;
using Basket.Dto;
using Basket.Common.Enums;
using Basket.Common.Helpers;

namespace Basket.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork uow) : base(uow) { }

        public async Task<ResponseDto<UserDto, UserReturnTypes>> CheckUser(string userName, string password)
        {
            // Password, User tablosundan ayrı bir tabloda kriptolanarak tutulmaktadır
            var entity = await GetQuery(p => p.Username == userName && !p.IsSoftDeleted && p.UserPassword.Password == CryptoHelper.CalculateMD5(password))
                .Include(p => p.UserPassword).FirstOrDefaultAsync();

            if (entity != null)
            {
                // Kullanıcı başarılı login oldu ise buraya girerek devamında Token oluşturacak

                var user = new UserDto() 
                {
                    Id = entity.Id,
                    FamilyName = entity.FamilyName,
                    GivenName = entity.GivenName,
                    Username = entity.Username
                };

                return await Task.FromResult(new ResponseDto<UserDto, UserReturnTypes>()
                {
                    IsSuccess = true,
                    ResponseCode = UserReturnTypes.Success,
                    Data = user
                });
            }

            return await Task.FromResult(new ResponseDto<UserDto, UserReturnTypes>()
            {
                IsSuccess = false,
                ResponseCode = UserReturnTypes.Err_NotValidUser,
                Data = null
            });
        }
    }
}
