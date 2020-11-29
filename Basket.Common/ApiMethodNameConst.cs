using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Common
{
    public class ApiMethodNameConst
    {
        // Api fonksiyonları için oluşturulan Router adresleridir

        public static class Basket
        {
            public const string GetBasket = "/api/Basket/GetBasket";
            public const string AddToBasket = "/api/Basket/AddToBasket";
        }

        public static class Access
        {
            public const string CreateToken = "/api/Access/CreateToken";
        }
    }
}
