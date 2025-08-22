using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeBook.Exceptions.ExceptionsBase;
using RecipeBook.Communication.Responses;
using RecipeBook.Exceptions;
using System;
using System.Net;

namespace RecipeBook.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is RecipeBookException recipeBookException)
                HandleProjectException(recipeBookException, context);
            else
                ThrowUnknowException(context);
        }

        /// <summary>
        ///     Posso criar um metodo privado somente dessa classe, que é usado no método principal como auxilio.
        /// </summary>
        /// <param name="context"></param>
        private static void HandleProjectException(RecipeBookException recipeBookException, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)recipeBookException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(recipeBookException.GetErrorMessages()));

            /*if (context.Exception is InvalidLoginException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(context.Exception.Message));
            }
            else if (context.Exception is ErrorOnValidationException)
            {
                var exception = context.Exception as ErrorOnValidationException;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception!.ErrorMessages));
            }
            else if (context.Exception is NotFoundException)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Result = new NotFoundObjectResult(new ResponseErrorJson(context.Exception.Message));
            }*/
        }

        private static void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
