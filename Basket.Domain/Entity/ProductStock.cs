using System;
using System.ComponentModel.DataAnnotations;

namespace Basket.Entity.Entity
{
    public class ProductStock
    {
        public virtual Product Product { get; set; }
        public virtual City City { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public int Stock { get; set; }
    }
}
