using AutoMapper;
using Basket.Common;
using Basket.Database.Context;
using Basket.Repository;
using Basket.Repository.Contracts;
using Basket.Service;
using Basket.Service.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using Web.Api.Config;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Basket.Common.Helpers;
using static Web.Api.Config.MapperConfig;

namespace Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthOptions>(Configuration.GetSection("AuthOptions"));

            var authOptions = Configuration.GetSection("AuthOptions").Get<AuthOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Headers["access_token"];
                            context.Token = accessToken;
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddSignalR();

            services.AddHealthChecks();

            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IBasketProductRepository, BasketProductRepository>();
            services.AddScoped<IProductStockRepository, ProductStockRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.ConnectionString = Configuration.GetConnectionString(ProjectConst.Configuration.DbConnection);
            connectionStringBuilder.Password = CryptoHelper.Base64ForUrlDecode(Configuration["Password"]);
            services.AddDbContext<BasketDbContext>(options => options.UseSqlServer(connectionStringBuilder.ToString()));

            services.AddDbContext<BasketDbContext>(options => options.UseSqlServer());
            services.AddScoped<DbContext, BasketDbContext>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web.Api", Version = "v1" });
            });

            // Dependency Inversion prensibine uygun olarak configure edilen AutoMapper
            // Dependency Injection sağlanmadığı durumlarda xUnit hata vermekteydi
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BasketDbContext db)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.Api v1"));

            db.Database.EnsureCreated();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
            });
        }
    }
}
