using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.ApiModel.RequestModels
{
    public class AddToBasketRequestModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Ürün seçimi yapınız.")]
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Ürün adedini belirtiniz.")]
        public int Quantity { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Sipariş şehrini belirtiniz (1-5)")]
        public int CityId { get; set; }
    }
}
