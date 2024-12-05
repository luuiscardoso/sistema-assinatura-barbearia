using APIAssinaturaBarbearia.Filtros;
using APIAssinaturaBarbearia.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestesAPI.IntegrationTests.CustomFactoryConfig
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.Test.json");
            });
;
            builder.ConfigureServices(services =>
            {
                var contextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BdContext>));
                ServiceProvider sp = services.BuildServiceProvider();

                if (contextDescriptor != null)
                {
                    services.Remove(contextDescriptor);
                }

                var configuration = sp.GetRequiredService<IConfiguration>();
                services.AddDbContext<BdContext>(op => op.UseSqlServer(configuration.GetConnectionString("TestConnection")));

                services.PostConfigure<HttpsRedirectionOptions>(options =>
                {
                    options.HttpsPort = 5001; // Define a porta HTTPS explicitamente
                });
            });
        }
    }
}
