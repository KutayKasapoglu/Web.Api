using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using Web.Api.Integraton.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Web.Api.Integraton.Tests.Controllers
{
    public class BasketControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public BasketControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateDefaultClient();
            _client.BaseAddress = new Uri("https://localhost:44300/api");
            _client.DefaultRequestHeaders.Add("access_token", CurrentToken.Token);
        }
    }
}
