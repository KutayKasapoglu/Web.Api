using System.ComponentModel;

namespace Basket.Common.Enums
{
    public enum GetBasketReturnTypes
    {
        [Description("Sepetinizde ürün bulunmamaktadır.")]
        Wrn_EmptyBasket,
        [Description("Sepet başarılı.")]
        Success,
        [Description("Sepet bulunamadı.")]
        Err_BasketNotFound,
        [Description("Beklenmedik bir hata oluştu.")]
        Err_UnexpedtedError
    }
}
