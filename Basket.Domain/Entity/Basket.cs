using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Basket.Entity.Entity
{
    public class Basket
    {
        public virtual User User { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        public virtual ICollection<BasketProduct> BasketProducts { get; set; }
    }
}
