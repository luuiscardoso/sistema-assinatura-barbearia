using APIAssinaturaBarbearia.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIAssinaturaBarbearia.Filtros
{
    public class FiltroExcecao : IExceptionFilter
    {
        public void OnException(ExceptionContext contextRequest)
        {
            if(contextRequest.Exception is NotFoundException)
            {
                contextRequest.Result = new NotFoundObjectResult(contextRequest.Exception.Message)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            } 

            if(contextRequest.Exception is AlreadyHasSubscriptionException)
            {
                contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            
     
            contextRequest.Result = new ObjectResult(contextRequest.Exception.Message)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
