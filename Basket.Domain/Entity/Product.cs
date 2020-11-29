using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basket.Entity.Entity
{
    public class Product
    {
        public virtual Currency Currency { get; set; }
        [Required]
        public int Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName ="decimal(12,4)")]
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        public int MinOrderQuantity { get; set; }
        public int MaxOrderQuantity { get; set; }
        [Required]
        public bool IsSoftDeleted { get; set; } = false;
        public virtual ICollection<ProductStock> ProductStocks { get; set; }
    }
}
