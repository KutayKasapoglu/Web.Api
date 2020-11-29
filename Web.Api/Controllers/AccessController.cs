using AutoMapper;
using Basket.ApiModel.RequestModels;
using Basket.ApiModel.ResponseModels;
using Basket.Common;
using Basket.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Api.Config;

// Authentication bu controller'da gerçekleştirilmektedir, bu nedenle proje çalışıtırıldığı zaman ilk olarak Token oluşturulmalıdır
// Authentication için Jason Web Token kullanılmaktadır
// Kullanıcı giriş bilgileri:
// Kullanıcı Adı: SeedUser - Şifre: password
// Kullanıcı Adı: SeedUser1/2/3/4/5 - Şifre: password1/2/3/4/5

namespace Web.Api.Controllers
{
    public class AccessController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccessController> _logger;
        private readonly IMapper _mapper;

        public AccessController(IUserService userService, ILogger<AccessController> logger, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost(ApiMethodNameConst.Access.CreateToken)]
        public async Task<IActionResult> CreateToken([FromBody] CreateTokenRequestModel model)
        {
            var user = await _userService.CheckUser(model.Username, model.Password);

            if (!user.IsSuccess)
            {
                // Hatalı kullanıcı giriş denemeleri log'lanmaktadır
                _logger.LogWarning($"Hatalı giriş denemesi :{ model.Username + " - " + model.Password}");
                return BadRequest(user.ResponseMessage);
            }

            var identity = await GetIdentity(_mapper.Map<UserResponseModel>(user.Data));

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.Issuer,
                    audience: AuthOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LifeTime)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
                useridentity = user.Data.Id
            };
            return Json(response);
        }

        private async Task<ClaimsIdentity> GetIdentity(UserResponseModel user)
        {
            var claims = new List<Claim>
                {
                    new Claim (ClaimsIdentity.DefaultNameClaimType, user.Username),
                    new Claim (ClaimsIdentity.DefaultIssuer, user.Id.ToString())
                };

            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
