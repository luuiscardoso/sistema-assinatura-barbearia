using APIAssinaturaBarbearia.Application.Exceptions;
using APIAssinaturaBarbearia.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APIAssinaturaBarbearia.Filtros
{
    public class FiltroExcecao : IExceptionFilter
    {
        public void OnException(ExceptionContext contextRequest)
        {
            switch (contextRequest.Exception)
            {
                case ApplicationNotFoundException:
                    contextRequest.Result = new NotFoundObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                    break;

                case ApplicationAlreadyHasSubscriptionException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case ApplicationUserNotRegisteredException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case ApplicationInvalidCredentialsException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case ApplicationInvalidRefreshTokenException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case ApplicationRevocationAccessException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case ApplicationUserAlreadyRegisteredException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case ApplicationRoleAlreadyExistsException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case ApplicationSearchPeriodOfInvalidDatesException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case ApplicationInvalidTokenException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case ApplicationNonMatchException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case DomainRenewalNotPaidException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                case DomainPeriodOfInvalidDatesException:
                    contextRequest.Result = new BadRequestObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;

                default:
                    contextRequest.Result = new ObjectResult(contextRequest.Exception.Message)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                    };
                    break;
            }
        }
    }
}
