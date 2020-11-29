using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

// JWT token bilgisi ile AccessController dışındaki diğer controller'lar buradan türeyecek
// Token bilgisi yanlış girildiği taktirde Controller'lara erişim gerçekleşmemektedir
// CurrentUser - UserId bilgisi login olunduktan sonra CurrentUser parametresine atanır, atama JWT Claims üzerinde taşınan bilgiler gerçekleştirilir

namespace Web.Api.Controllers
{
    public class BaseController : Controller
    {
        // Sign In olduktan sonra Token bilgisi access_token parametresi içerisinde tutuluyor
        [FromHeader(Name = "access_token")]
        public string token
        {
            get
            {
                return Request.Headers["access_token"];
            }
        }

        // Access dışındaki tüm Controller ve Metot'lar CurrentUser bilgilerine CurrentUserId üzerinden erişebilmektedir
        public Guid CurrentUserId
        {
            get
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                return Guid.Parse(identity.Claims.ToList()[1].Value);
            }
        }
    }
}
