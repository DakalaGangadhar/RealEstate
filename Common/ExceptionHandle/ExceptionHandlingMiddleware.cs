using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Common.ExceptionHandle
{
    /// <summary>
    /// Global exception handling middleware.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoke http context async.
        /// </summary>
        /// <param name="context">HttpContext request.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Exception handling async method.
        /// </summary>
        /// <param name="context">HttpContext request.</param>
        /// <param name="ex">Exception request.</param>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResponse = new
            {
                Message = "An unexpected error occurred. Please try again later.",
                // You can add more details in development mode, but avoid exposing sensitive information in production.
                // Details = ex.ToString() //Only for development.
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}