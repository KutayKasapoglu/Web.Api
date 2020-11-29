using Basket.Dto.Dto;
using System.Collections.Generic;

namespace Basket.Dto.Dto
{
    public class BasketDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalPrice { get; set; }
        public string CurrencyName { get; set; }
        public List<BasketProductDto> Products { get; set; }
    }
}
