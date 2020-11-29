
using System.ComponentModel.DataAnnotations;

namespace Basket.Entity.Entity
{
    public class Currency
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(5)]
        [Required]
        public string Name { get; set; }
        [Required]
        public char Sign { get; set; }
    }
}
