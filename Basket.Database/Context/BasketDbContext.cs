using Basket.Entity.Entity;
using Microsoft.EntityFrameworkCore;

namespace Basket.Database.Context
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext() { }
        public BasketDbContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<Entity.Entity.Basket> Baskets { get; set; }
        public virtual DbSet<BasketProduct> BasketProducts { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserPassword> UserPasswords { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<ProductStock> ProductStocks { get; set; }
    }
}
