using Microsoft.AspNetCore.Http;
using Oyooni.Server.Common;
using Oyooni.Server.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Oyooni.Server.Middlewares
{
    /// <summary>
    /// Represents a global exception handling middleware
    /// </summary>
    public class ExceptionsHandlingMiddleware : IMiddleware
    {
        /// <summary>
        /// Invoked when the middleware is called
        /// </summary>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Go to the next middelware
                await next(context);
            }
            catch (Exception ex)
            {
                // Handle the exception whatever it was
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Handles the passed exception
        /// </summary>
        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Initial status code
            var statusCode = (int)HttpStatusCode.InternalServerError;

            // Initial message
            var message = "Server error";

            // If the exception was a base application exception
            if (ex is BaseException baseException)
            {
                // Get the status code
                statusCode = baseException.StatusCode;

                // Get the message
                message = baseException.Message;
            }

            // Create an api error response and jsonize it
            var result = new ApiErrorResponse(message).ToJson();

            // Set the content type
            context.Response.ContentType = "application/json";

            // Set the status code
            context.Response.StatusCode = statusCode;

            // Write the error response
            await context.Response.WriteAsync(result);
        }
    }
}
