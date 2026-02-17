using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using System.Threading.Tasks;

namespace APIAssinaturaBarbearia.CrossCutting.Security
{
    static public class RateLimiterConfiguration
    {
        static public void ConfigureRateLimiter(this IServiceCollection services, IConfiguration config)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext =>
                                        RateLimitPartition.GetFixedWindowLimiter(
                                                           partitionKey: httpcontext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ??
                                                                         httpcontext.Request.Headers.Host.ToString(),
                                        factory: partition => new FixedWindowRateLimiterOptions
                                        {
                                            AutoReplenishment = true,
                                            PermitLimit = 3,
                                            QueueLimit = 0,
                                            Window = TimeSpan.FromSeconds(5)
                                        }));
            });
        }
    }
}
