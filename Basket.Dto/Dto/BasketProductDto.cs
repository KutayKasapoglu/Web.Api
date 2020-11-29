
namespace Basket.Dto.Dto
{
    public class BasketProductDto
    {
        public int Id { get; set; }
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string CurrencyName { get; set; }
        public int CurrencyId { get; set; }
    }
}
