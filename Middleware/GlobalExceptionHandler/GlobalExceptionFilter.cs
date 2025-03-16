using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Middleware.GlobalExceptionHandler
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            // Log the error details
            _logger.LogError(context.Exception, "An error occurred in the application");

            // Create an error response object
            var errorResponse = new
            {
                Success = false,
                Message = "An unexpected error occurred. Please try again later.",
                Error = context.Exception.Message
            };

            // Return a structured JSON response with a 500 status code
            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500
            };

            // Mark the exception as handled
            context.ExceptionHandled = true;
        }
    }
}