using System.Net;
using System.Text.Json;
using IntegrationApi.Core.Exceptions;

namespace IntegrationApi.Api.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly bool _isDevelopment;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _isDevelopment = env.IsDevelopment();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // If the exception is a known business exception just log like info
                if (ex is ResourceNotFoundException || ex is ValidationException)
                {
                    _logger.LogInformation("Business exception: {Message}", ex.Message);
                }
                else
                {
                    _logger.LogError(ex, "Unhandled error: {Message}", ex.Message);
                }
                
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Determine the HTTP status code based on the exception type
            var statusCode = exception switch
            {
                ValidationException _ => HttpStatusCode.BadRequest,
                ResourceNotFoundException _ => HttpStatusCode.NotFound,
                ExternalApiException _ => HttpStatusCode.BadGateway,
                ApplicationException _ => HttpStatusCode.BadRequest,
                KeyNotFoundException _ => HttpStatusCode.NotFound,
                UnauthorizedAccessException _ => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError
            };
            
            context.Response.StatusCode = (int)statusCode;

            // Build a response object with relevant details
            object response;
            
            if (exception is ValidationException validationException)
            {
                response = new
                {
                    status = statusCode,
                    message = exception.Message,
                    propertyName = validationException.PropertyName
                };
            }
            else if (exception is ExternalApiException apiException)
            {
                response = new
                {
                    status = statusCode,
                    message = exception.Message,
                    apiName = apiException.ApiName,
                    endpoint = apiException.Endpoint
                };
            }
            else
            {
                response = new
                {
                    status = statusCode,
                    message = exception.Message,
                    detalhes = _isDevelopment ? exception.InnerException?.Message : null
                };
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
