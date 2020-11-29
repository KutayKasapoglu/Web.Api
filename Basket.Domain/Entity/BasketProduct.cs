using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basket.Entity.Entity
{
    public class BasketProduct
    {
        public virtual Basket Basket { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public int BasketId { get; set; }
        [Required]
        public int  ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(12,4)")]
        public decimal CurrencyRate { get; set; }
        [Required]
        public bool IsSoftDeleted { get; set; } = false;
    }
}
