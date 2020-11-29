using Microsoft.AspNetCore.Hosting;
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

namespace Web.Api.IntegrationTests.Controllers
{
    public class AccessControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        // AccessController Integration Test metodu burada gerçekleştirilmektedir

        private readonly HttpClient _client;
        public AccessControllerTests(WebApplicationFactory<Startup> factory)
        {
            // Integration Test için Testing ortamı ayağa kullanılıyor
            _client = factory.WithWebHostBuilder(conf => conf.UseEnvironment("Testing")).CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5005/api/");
        }

        [Fact]
        public async Task Token_ShouldReturnSuccessStatusCode()
        {
            var parameters = new Dictionary<string, string>
            {
                { "username", "TestUser" },
                { "password", "password" }
            };

            string body = JsonConvert.SerializeObject(parameters);
            var response = await _client.PostAsync("access/createtoken", new StringContent(body, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            // Oluşturulan Token diğer testlerde kullanılmak adına static parametreye atanıyor
            string responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<JObject>(responseString);
            CurrentToken.Token = (string)responseObject["access_token"];
        }
    }
}
