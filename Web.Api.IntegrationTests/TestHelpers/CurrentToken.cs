using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.IntegrationTests.TestHelpers
{
    public static class CurrentToken
    {
        // CreateToken Test metodu ile elde edilen Token'in bu static değişkende tutulması ile testlerde JWT Authentication doğrulaması yapılaktadır

        public static string Token { get; set; }
    }
}
