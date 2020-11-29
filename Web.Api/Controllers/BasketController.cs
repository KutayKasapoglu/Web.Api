using AutoMapper;
using Basket.ApiModel;
using Basket.ApiModel.RequestModels;
using Basket.ApiModel.ResponseModels;
using Basket.Common;
using Basket.Common.Enums;
using Basket.Dto.RequestDto;
using Basket.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// Api ile iletişim ApiModel'ler aracılığyla gerçekleştiriliyor - servis'e giderken mapper ile ilgili Dto modelleri oluşturularak gönderiliyor
// Api dönüşleri ApiResponseModel kullanılarak gerçekleştirilmektedir, içerisinde sorgunun başarı durumu, döndürülecek model ve hata-uyarı bilgileri bulunmaktadır
// Kullanıcı bilgileri BaseController üzerinden JWWT kullanılarak alınmaktadır

namespace Web.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;

        public BasketController(IBasketService basketService, IMapper mapper)
        {
            _basketService = basketService;
            _mapper = mapper;
        }

        [HttpGet(ApiMethodNameConst.Basket.GetBasket)]
        public async Task<ApiResponseModel<BasketResponseModel, GetBasketReturnTypes>> GetBasket()
        {
            // Oluşturulmuş bir Basket var ise Token oluşturulduktan sonra bu metot ile erişilecektir
            // Basket henüz oluşturulmamışsa "Sepet bulunamadı." uyarısı döner

            // User bilgiler Token'de tutulmaktadır, bu nedenle UserId girişi yapmaya gerek yoktur
            var data = await _basketService.GetBasket(CurrentUserId);

            return new ApiResponseModel<BasketResponseModel, GetBasketReturnTypes>()
            {
                IsSuccess = data.IsSuccess,
                ResponseCode = data.ResponseCode,
                ResponseMessage = data.ResponseMessage,
                Data = _mapper.Map<BasketResponseModel>(data.Data)
            };
        }

        [HttpPost(ApiMethodNameConst.Basket.AddToBasket)]
        public async Task<ApiResponseModel<bool, AddToBasketReturnTypes>> AddToBasket([FromBody] AddToBasketRequestModel requestModel)
        {
            // Ürünün Basket'e eklenmesi bu metot ile gerçekleştirilmektedir
            // Burada girilen Şehir-Adet-Ürün bilgilerine göre Stock Şehir'de var mı, Min-Max Order Quantity'e uyuyor mu ve mevcut Stock var mı diye kontrol ederek
            // sepete başarı ile ekler ya da warning döner
            // Stock başarılı dönüş verirse Basket'e ekleme ve üründe Stock indirme işlemleri gerçekleştirilir
            // Kullanıcı için aktif bir Basket yok ise, bu işlemler sırasında yeni Basket oluşturulur

            var requestDto = _mapper.Map<AddToBasketRequestDto>(requestModel);
            var data = await _basketService.AddToBasket(CurrentUserId, requestDto);

            return new ApiResponseModel<bool, AddToBasketReturnTypes>()
            {
                IsSuccess = data.IsSuccess,
                ResponseCode = data.ResponseCode,
                ResponseMessage = data.ResponseMessage
            };
        }
    }
}
