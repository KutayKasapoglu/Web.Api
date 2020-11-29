using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

                //.ConfigureAppConfiguration((context, config) =>
                //{
                //    using (var store = new X509Store(StoreLocation.CurrentUser))
                //    {
                //        store.Open(OpenFlags.ReadOnly);
                //        var certs = store.Certificates
                //            .Find(X509FindType.FindByThumbprint,
                //                "f08f0774-cf25-4b08-9a64-5c02e6e87694", false);

                //        config.AddAzureKeyVault(
                //            $"https://ciceksepeti.vault.azure.net/",
                //            "92b93bdd-24fe-418f-b107-09fd8240b4d8",
                //            certs.OfType<X509Certificate2>().Single());

                //        store.Close();
                //    }
                //});
    }
}
