using Basket.Repository.Contracts;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace Basket.Service.Tests.Helpers
{
    public class ConstraintHelper
    {
        // Unit Test Constraint'leri için ortak static değişkenler oluşturdu 
        public static Guid UserId { get; set; } = Guid.NewGuid();

        public static Mock<IProductStockRepository> mockProductStock { get; set; } = new Mock<IProductStockRepository>();
        public static Mock<IBasketRepository> mockBasket { get; set; } = new Mock<IBasketRepository>();
        public static Mock<IBasketProductRepository> mockBasketProduct { get; set; } = new Mock<IBasketProductRepository>();
        public static Mock<IUnitOfWork> mockUnitOfWork { get; set; } = new Mock<IUnitOfWork>();
        public static Mock<ILogger<BasketService>> mockLogger { get; set; } = new Mock<ILogger<BasketService>>();
        public static Mock<IDbContextTransaction> mockContextTransaction { get; set; } = new Mock<IDbContextTransaction>();
    }
}
