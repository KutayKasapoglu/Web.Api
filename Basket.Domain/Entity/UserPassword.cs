using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Entity.Entity
{
    public class UserPassword
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Password { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
