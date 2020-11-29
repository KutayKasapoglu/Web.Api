using Basket.Common.Enums;
using Basket.Dto.Dto;
using Basket.Dto.RequestDto;
using Basket.Entity.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Basket.Service.Tests.Helpers;

namespace Basket.Service.Tests
{
    [TestClass]
    public class AddToBasketShould
    {
        // AddToBasket servis katmanı örnek Unit Test metotları burada gerçekleştirilmektedir

        [TestMethod]
        public void NotAddProductDueToLackOfStock()
        {
            //arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "product"
            };

            var requestDto = new AddToBasketRequestDto()
            {
                CityId = 1,
                ProductId = 1,
                Quantity = 1
            };

            ConstraintHelper.mockProductStock.Setup(x => x.GetProductForCityId(1, 1, 1)).Returns(Task.FromResult(productDto));

            //act
            var service = new BasketService(ConstraintHelper.mockBasketProduct.Object, ConstraintHelper.mockBasket.Object, ConstraintHelper.mockProductStock.Object, ConstraintHelper.mockUnitOfWork.Object, ConstraintHelper.mockLogger.Object);
            var actual = service.AddToBasket(ConstraintHelper.UserId, requestDto);

            Assert.AreEqual(AddToBasketReturnTypes.Err_ProductNotFound, actual.Result.ResponseCode);
            Assert.AreEqual(false, actual.Result.IsSuccess);
            Assert.AreEqual(false, actual.Result.Data);
        }

        [TestMethod]
        public void NotAddProductDueToMissingProduct()
        {
            var requestDto = new AddToBasketRequestDto()
            {
                CityId = 1,
                ProductId = 0,
                Quantity = 1
            };

            ConstraintHelper.mockProductStock.Setup(x => x.GetProductForCityId(0, 1, 1)).Returns(Task.FromResult(new ProductDto()));

            var service = new BasketService(ConstraintHelper.mockBasketProduct.Object, ConstraintHelper.mockBasket.Object, ConstraintHelper.mockProductStock.Object, ConstraintHelper.mockUnitOfWork.Object, ConstraintHelper.mockLogger.Object);
            var actual = service.AddToBasket(ConstraintHelper.UserId, requestDto);

            Assert.AreEqual(AddToBasketReturnTypes.Err_ProductNotFound, actual.Result.ResponseCode);
            Assert.AreEqual(false, actual.Result.IsSuccess);
            Assert.AreEqual(false, actual.Result.Data);
        }

        [TestMethod]
        public void NotAddProductDueToLackOfQuantityInsert()
        {
            var requestDto = new AddToBasketRequestDto()
            {
                CityId = 1,
                ProductId = 1,
                Quantity = 0
            };

            ConstraintHelper.mockProductStock.Setup(x => x.GetProductForCityId(1, 1, 0)).Returns(Task.FromResult(new ProductDto()));

            var service = new BasketService(ConstraintHelper.mockBasketProduct.Object, ConstraintHelper.mockBasket.Object, ConstraintHelper.mockProductStock.Object, ConstraintHelper.mockUnitOfWork.Object, ConstraintHelper.mockLogger.Object);
            var actual = service.AddToBasket(ConstraintHelper.UserId, requestDto);

            Assert.AreEqual(AddToBasketReturnTypes.Err_ProductNotFound, actual.Result.ResponseCode);
            Assert.AreEqual(false, actual.Result.IsSuccess);
            Assert.AreEqual(false, actual.Result.Data);
        }

        [TestMethod]
        public void SuccessfullyAddProduct()
        {
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "product",
                Stock = new StockDto() { Id = 1, CityId = 1, Quantity = 10 },
                MinOrderQuantity = 1,
                MaxOrderQuantity = 100
            };

            var basketId = 1;

            var basketProductEntity = new BasketProduct()
            {
                BasketId = 1,
                ProductId = 1,
                Quantity = 2
            };

            var productResponseDto = new ProductDto()
            {
                Id = 1,
                Stock = new StockDto() { Id = 1, CityId = 1, Quantity = 8 },
            };

            var requestDto = new AddToBasketRequestDto()
            {
                CityId = 1,
                ProductId = 1,
                Quantity = 2
            };

            ConstraintHelper.mockProductStock.Setup(x => x.GetProductForCityId(1, 1, 2)).Returns(Task.FromResult(productDto));
            ConstraintHelper.mockBasket.Setup(x => x.GetOrCreateBasketId(ConstraintHelper.UserId)).Returns(Task.FromResult(basketId));
            ConstraintHelper.mockBasketProduct.Setup(x => x.AddToBasket(basketProductEntity));
            ConstraintHelper.mockBasketProduct.Setup(x => x.BeginTransaction()).Returns(ConstraintHelper.mockContextTransaction.Object);
            ConstraintHelper.mockProductStock.Setup(x => x.UpdateStock(1, 8));

            var service = new BasketService(ConstraintHelper.mockBasketProduct.Object, ConstraintHelper.mockBasket.Object, ConstraintHelper.mockProductStock.Object, ConstraintHelper.mockUnitOfWork.Object, ConstraintHelper.mockLogger.Object);
            var actual = service.AddToBasket(ConstraintHelper.UserId, requestDto);

            Assert.AreEqual(true, actual.Result.IsSuccess);
            Assert.AreEqual(AddToBasketReturnTypes.Success, actual.Result.ResponseCode);
        }

    }
}