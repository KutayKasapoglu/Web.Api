using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Web.Api.Config
{
    public class AuthOptions
    {
        // Tason Web Token Authentication modeli
        public const string Issuer = "KutayKasapoglu";
        public const string Audience = "CicekSepeti";
        const string SecureKey = "GetSymmetricSecurityKey";
        public const int LifeTime = 10;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecureKey));
        }
    }
}
