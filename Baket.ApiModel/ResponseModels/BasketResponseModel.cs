using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.ApiModel.ResponseModels
{
    public class BasketResponseModel
    {
        public int Id { get; set; }
        public BasketUserResponseModel User { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalPrice { get; set; }
        public string CurrencyName { get; set; }
        public List<BasketProductResponseModel> Products { get; set; }
    }

    public class BasketUserResponseModel {
        public Guid Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
    }

    public class BasketProductResponseModel
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
