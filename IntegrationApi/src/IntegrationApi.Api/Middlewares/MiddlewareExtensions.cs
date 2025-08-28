using Microsoft.AspNetCore.Builder;

namespace IntegrationApi.Api.Middlewares
{
    /// <summary>
    /// Extension to add the middleware in application
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adiciona o middleware de tratamento global de exceções ao pipeline
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
