using Basket.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Basket.Entity.Entity
{
    public class User
    {
        public virtual UserPassword UserPassword { get; set; }
        [Required]
        public Guid Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string GivenName { get; set; }
        [MaxLength(50)]
        [Required]
        public string FamilyName { get; set; }
        [MaxLength(50)]
        [Required]
        public string Username { get; set; }
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }
        [Required]
        public int UserPasswordId { get; set; }
        [Required]
        public bool IsSoftDeleted { get; set; } = false;
    }
}
