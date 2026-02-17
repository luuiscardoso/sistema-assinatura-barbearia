using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIAssinaturaBarbearia.CrossCutting.Middlewares;

public class ValidateSearchFieldsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _fields;
    public ValidateSearchFieldsMiddleware(RequestDelegate next)
    {
        _next = next;
        _fields = new HashSet<string>(["cpf", "email", "dataInicio", "dataFinal", "status", "nome"]);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(!HttpMethods.IsGet(context.Request.Method))
        {
            await _next(context);
            return;
        }

        var queryKeys = context.Request.Query.Keys;

        var invalidFields = queryKeys.Where(q => !_fields.Contains(q.ToLowerInvariant()));

        if (invalidFields.Count() > 0)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Invalid search parameters",
                Detail = "One or more fields are not supposed to be used as filter parameters. Valid fields: CPF, Status, Name, Date",
                Status = StatusCodes.Status400BadRequest
            });

            return;
        }

        await _next(context);
    }
}
