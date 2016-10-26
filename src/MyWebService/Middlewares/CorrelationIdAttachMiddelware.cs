using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NLog;

namespace MyWebService.Middlewares
{
    /// <summary>
    /// Middleware to ensure correlation ID set in context for every request scope
    /// </summary>
    public class CorrelationIdAttachMiddelware
    {
        private const string CorrelationIdHeaderKey = "x-correlation-id";

        private readonly RequestDelegate _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public CorrelationIdAttachMiddelware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            // Setup correslation ID in the context
            var correlationId = GetCorrelationId(context);
            MappedDiagnosticsLogicalContext.Set("correlationId", correlationId);

            // Attach the correlation ID to reponse header
            context.Response.Headers[CorrelationIdHeaderKey] = correlationId;

            await _next.Invoke(context);
        }

        /// <summary>
        /// Get or generate a correlation ID based on the context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string GetCorrelationId(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(CorrelationIdHeaderKey)
                && !string.IsNullOrEmpty(context.Request.Headers[CorrelationIdHeaderKey]))
            {
                return context.Request.Headers[CorrelationIdHeaderKey];
            }
            else
            {
                return context.TraceIdentifier;
            }
        }
    }
}
