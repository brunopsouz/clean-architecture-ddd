﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecipeBook.Communication.Responses;
using RecipeBook.Exceptions;
using RecipeBook.Exceptions.ExceptionsBase;
using System;
using System.Net;

namespace RecipeBook.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is RecipeBookException)
                HandleProjectException(context);
            else
                ThrowUnknowException(context);
        }

        /// <summary>
        ///     Posso criar um metodo privado somente dessa classe, que é usado no método principal como auxilio.
        /// </summary>
        /// <param name="context"></param>
        private void HandleProjectException(ExceptionContext context)
        {
            if(context.Exception is ErrorOnValidationException)
            {
                var exception = context.Exception as ErrorOnValidationException;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception.ErrorMessages));
            }
        }

        private void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
