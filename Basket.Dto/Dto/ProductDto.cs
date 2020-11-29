using System.Collections.Generic;

namespace Basket.Dto.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public int MinOrderQuantity { get; set; }
        public int MaxOrderQuantity { get; set; }
        public StockDto Stock { get; set; }
    }
}
