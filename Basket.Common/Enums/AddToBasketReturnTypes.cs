using System.ComponentModel;

namespace Basket.Common.Enums
{
    public enum AddToBasketReturnTypes
    {
        [Description("Sepetinizde ürün bulunmamaktadır.")]
        Wrn_EmptyBasket,
        [Description("Seçili ürün için stok bulunmamaktadır.")]
        Wrn_LackOfStock,
        [Description("Minimum spiariş miktarı sağlanmamıştır.")]
        Wrn_MinOrderQuantity,
        [Description("Maskimum spiariş miktarı sağlanmamıştır.")]
        Wrn_MaxOrderQuantity,
        [Description("Beklenmedik bir hata oluştu.")]
        Err_UnexpedtedError,
        [Description("Sepete eklenecek ürün/adet eşleşmesi bulunamadı.")]
        Err_ProductNotFound,
        [Description("En az 1 ürün eklenmesi gerekmektedir.")]
        Err_WrongQuantity,
        [Description("Ürün başarıyla sepete eklendi.")]
        Success,
        [Description("Kullanıcı bulunamadı.")]
        Err_NotValidUser,
        [Description("Sepetinizde başka şehir ürünü bulunduğu için eklenemedi.")]
        Wrn_UnMatchedCity
    }
}
