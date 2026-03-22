using System.Text.Json;

namespace ExpenseTracker.API.Middleware
{
    using ExpenseTracker.BusinessLogic.Common;
    using ExpenseTracker.BusinessLogic.Exceptions;
    using System.Text.Json;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,
                                   ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            int statusCode = StatusCodes.Status500InternalServerError;

            var response = new ApiResponse<object>
            {
                success = false,
                message = "An unexpected error occurred",
                errorCode = "SERVER_ERROR"
            };

            if (exception is ApiException apiException)
            {
                statusCode = apiException.StatusCode;

                response = new ApiResponse<object>
                {
                    success = false,
                    message = apiException.Message,
                    errorCode = apiException.ErrorCode
                };
            }

            _logger.LogError(exception, "Unhandled Exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
