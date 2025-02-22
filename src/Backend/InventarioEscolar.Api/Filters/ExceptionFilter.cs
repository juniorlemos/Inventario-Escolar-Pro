using InventarioEscolar.Communication.Response;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InventarioEscolar.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is InventarioEscolarException inventarioException)
                HandleProjectException(inventarioException, context);
            else
                ThrowUnknowException(context);
        }

        private static void HandleProjectException(InventarioEscolarException inventarioException, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)inventarioException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(inventarioException.GetErrorMessages()));
        }

        private static void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
