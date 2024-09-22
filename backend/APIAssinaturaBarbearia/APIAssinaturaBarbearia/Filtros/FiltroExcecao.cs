using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIAssinaturaBarbearia.Filtros
{
    public class FiltroExcecao : IExceptionFilter
    {
        public void OnException(ExceptionContext contextRequest)
        {
            contextRequest.Result = new ObjectResult(contextRequest.Exception.Message)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
