using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace APIAssinaturaBarbearia.CrossCutting.Middlewares.GlobalExceptions
{
    public static class ExcessaoMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async requestContext =>
                {
                    requestContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    requestContext.Response.ContentType = "application/json";

                    var contextFeature = requestContext.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var obj = new
                        {
                            requestContext.Response.StatusCode,
                            contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace
                        };

                        await requestContext.Response.WriteAsync(obj.ToString());
                    }
                });
            });
        }
    }
}
