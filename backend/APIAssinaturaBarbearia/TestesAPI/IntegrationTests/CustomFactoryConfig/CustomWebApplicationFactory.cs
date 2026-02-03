using APIAssinaturaBarbearia.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.RateLimiting;

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
   
            builder.ConfigureServices(services =>
            {
                var contextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BdContext>));
                ServiceProvider sp = services.BuildServiceProvider();

                if (contextDescriptor != null)
                {
                    services.Remove(contextDescriptor);
                }

                var configuration = sp.GetRequiredService<IConfiguration>();

                services.AddRateLimiter(options =>
                {
                    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
                                        RateLimitPartition.GetFixedWindowLimiter(
                                                           partitionKey: Guid.NewGuid().ToString(),
                                        factory: partition => new FixedWindowRateLimiterOptions
                                        {
                                            AutoReplenishment = true,
                                            PermitLimit = configuration.GetSection("RateLimiter").GetValue<int>("LimiteRequisicoes"),
                                            QueueLimit = configuration.GetSection("RateLimiter").GetValue<int>("LimiteFila"),
                                            Window = TimeSpan.FromSeconds(configuration.GetSection("RateLimiter").GetValue<double>("Janela"))
                                        }));
                });


                services.AddDbContext<BdContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                services.PostConfigure<HttpsRedirectionOptions>(options =>
                {
                    options.HttpsPort = 5001; 
                });
            });
        }
    }
}
