using Microsoft.AspNetCore.Builder;

namespace MyWebService.Middlewares
{
    /// <summary>
    /// Extension to register request logger to the application
    /// </summary>
    public static class MiddlewareRegistrationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdAttachMiddelware>();
        }
    }
}
