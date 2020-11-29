using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Web.Api.IntegrationTests.TestHelpers;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Json;
using Basket.ApiModel;
using Basket.Common.Enums;
using Basket.ApiModel.ResponseModels;
using Basket.ApiModel.RequestModels;

namespace Web.Api.IntegrationTests.Controllers
{
    public class BasketControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        // BasketController Integration Test metotları burada gerçekleştirilmektedir

        private readonly HttpClient _client;

        public BasketControllerTests(WebApplicationFactory<Startup> factory)
        {
            // Integration Test için Testing ortamı ayağa kullanılıyor
            _client = factory.WithWebHostBuilder(conf => conf.UseEnvironment("Testing")).CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5005/api/");
            _client.DefaultRequestHeaders.Add("access_token", CurrentToken.Token);
        }

        [Fact]
        public async Task Token_ShouldReturnSuccessStatusCode()
        {
            // Async olarak çalıştıkları için AccessControllerTest token oluşturmadan burası başlayabiliyor
            // Bu nedenle Authorization hatası alınıyor
            // Aynı test metodunu buraya da ekleyerek testin aralıksız olarak çalışması sağlandı

            // Assert
            var parameters = new Dictionary<string, string>
            {
                { "username", "SeedUser" },
                { "password", "password" }
            };

            // Act
            string body = JsonConvert.SerializeObject(parameters);
            var response = await _client.PostAsync("access/createtoken", new StringContent(body, Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode();

            // Oluşturulan Token diğer testlerde kullanılmak adına static parametreye atanıyor
            string responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<JObject>(responseString);
            CurrentToken.Token = (string)responseObject["access_token"];
        }

        [Fact]
        public async Task GetBasket_ShouldReturnSuccessfully()
        {
            var responseModel = await _client.GetFromJsonAsync<ApiResponseModel<BasketResponseModel, GetBasketReturnTypes>>("basket/getbasket");

            Assert.NotNull(responseModel);
            Assert.True(responseModel.IsSuccess);
            Assert.True(responseModel.Data.Products.Count >= 3);
            Assert.Equal(GetBasketReturnTypes.Success, responseModel.ResponseCode);
        }

        [Fact]
        public async Task AddToBasket_ShouldReturnLackOfProduct()
        {
            var requestModel = new AddToBasketRequestModel()
            {
                ProductId = 4,
                Quantity = 10,
                CityId = 1
            };

            var response = await _client.PostAsJsonAsync("basket/addtobasket", requestModel, JsonSerializerHelper.DefaultSerialisationOptions);
            var responseModel = await response.Content.ReadFromJsonAsync<ApiResponseModel<bool, AddToBasketReturnTypes>>();

            Assert.False(responseModel.IsSuccess);
            Assert.Equal(AddToBasketReturnTypes.Err_ProductNotFound, responseModel.ResponseCode);
        }

        [Fact]
        public async Task AddToBasket_ShouldReturnSuccessfully()
        {
            var requestModel = new AddToBasketRequestModel()
            {
                ProductId = 1,
                Quantity = 1,
                CityId = 1
            };

            var response = await _client.PostAsJsonAsync("basket/addtobasket", requestModel, JsonSerializerHelper.DefaultSerialisationOptions);
            var responseModel = await response.Content.ReadFromJsonAsync<ApiResponseModel<bool, AddToBasketReturnTypes>>();

            Assert.True(responseModel.IsSuccess);
            Assert.Equal(AddToBasketReturnTypes.Success, responseModel.ResponseCode);
        }

    }
}
