using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Web.Api.IntegrationTests
{
    public class HealthCheckTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        // Api HealtCheck Integration Test metodu

        private readonly HttpClient _client;

        public HealthCheckTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.WithWebHostBuilder(conf => conf.UseEnvironment("Testing")).CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5005/api/");
        }

        [Fact]
        public async Task HealthCheck_ReturnOk()
        {
            var response = await _client.GetAsync("/healthcheck");
            response.EnsureSuccessStatusCode();
        }
    }
}
