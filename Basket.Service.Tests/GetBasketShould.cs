using Basket.Common.Enums;
using Basket.Dto;
using Basket.Dto.Dto;
using Basket.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Service.Tests.Helpers;

namespace Basket.Service.Tests
{
    [TestClass]
    public class GetBasketShould
    {
        // GetBasket servis katmanı örnek Unit Test metotları burada gerçekleştirilmektedir

        [TestMethod]
        public void NotGetBasketDueToLackOfBasket()
        {
            var responseBasketDto = Task.FromResult(new ResponseDto<BasketDto, GetBasketReturnTypes>());
            responseBasketDto.Result.Data = null;
            responseBasketDto.Result.IsSuccess = false;
            responseBasketDto.Result.ResponseCode = GetBasketReturnTypes.Err_BasketNotFound;

            ConstraintHelper.mockBasket.Setup(x => x.GetBasket(ConstraintHelper.UserId)).Returns(responseBasketDto);

            var service = new BasketService(ConstraintHelper.mockBasketProduct.Object, ConstraintHelper.mockBasket.Object, ConstraintHelper.mockProductStock.Object, ConstraintHelper.mockUnitOfWork.Object, ConstraintHelper.mockLogger.Object);
            var actual = service.GetBasket(ConstraintHelper.UserId);

            Assert.AreEqual(false, actual.Result.IsSuccess);
            Assert.AreEqual(GetBasketReturnTypes.Err_BasketNotFound, actual.Result.ResponseCode);
            Assert.AreEqual(null, actual.Result.Data);
        }

        [TestMethod]
        public void GetEmptyBasket()
        {
            var basketDto = new BasketDto()
            {
                Id = 1,
                User = new UserDto()
                {
                    Id = ConstraintHelper.UserId,
                },
                IsActive = true,
                Products = new List<BasketProductDto>() { }
            };

            var responseBasketDto = Task.FromResult(new ResponseDto<BasketDto, GetBasketReturnTypes>());
            responseBasketDto.Result.Data = basketDto;
            responseBasketDto.Result.IsSuccess = false;
            responseBasketDto.Result.ResponseCode = GetBasketReturnTypes.Wrn_EmptyBasket;

            ConstraintHelper.mockBasket.Setup(x => x.GetBasket(ConstraintHelper.UserId)).Returns(responseBasketDto);

            var service = new BasketService(ConstraintHelper.mockBasketProduct.Object, ConstraintHelper.mockBasket.Object, ConstraintHelper.mockProductStock.Object, ConstraintHelper.mockUnitOfWork.Object, ConstraintHelper.mockLogger.Object);
            var actual = service.GetBasket(ConstraintHelper.UserId);

            Assert.AreEqual(GetBasketReturnTypes.Wrn_EmptyBasket, actual.Result.ResponseCode);
            Assert.AreEqual(basketDto, actual.Result.Data);
        }

        [TestMethod]
        public void GetSuccessfully()
        {
            var basketDto = new BasketDto()
            {
                Id = 1,
                User = new UserDto()
                {
                    Id = ConstraintHelper.UserId,
                },
                IsActive = true,
                Products = new List<BasketProductDto>()
                {
                    new BasketProductDto() { Id = 1, ProductId = 1, BasketId = 1, Quantity = 1},
                    new BasketProductDto() { Id = 2, ProductId = 2, BasketId = 2, Quantity = 2}
                }
            };

            var responseBasketDto = Task.FromResult(new ResponseDto<BasketDto, GetBasketReturnTypes>());
            responseBasketDto.Result.Data = basketDto;
            responseBasketDto.Result.IsSuccess = true;

            ConstraintHelper.mockBasket.Setup(x => x.GetBasket(ConstraintHelper.UserId)).Returns(responseBasketDto);

            var service = new BasketService(ConstraintHelper.mockBasketProduct.Object, ConstraintHelper.mockBasket.Object, ConstraintHelper.mockProductStock.Object, ConstraintHelper.mockUnitOfWork.Object, ConstraintHelper.mockLogger.Object);
            var actual = service.GetBasket(ConstraintHelper.UserId);

            Assert.AreEqual(true, actual.Result.IsSuccess);
            Assert.AreEqual(basketDto, actual.Result.Data);
        }
    }
}