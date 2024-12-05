using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace APIAssinaturaBarbearia.Filtros
{
    public static class ExcessaoMidd
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
                            StatusCode = requestContext.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace
                        };

                        await requestContext.Response.WriteAsync(obj.ToString());
                    }
                });
            });
        }
    }
}
