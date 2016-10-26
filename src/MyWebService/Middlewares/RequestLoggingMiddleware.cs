using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NLog;
using NuGet.Protocol.Core.v3;

namespace MyWebService.Middlewares
{
    /// <summary>
    /// A middleware to log all requests
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            // Log request
            await LogRequest(context.Request);

            // Invoke the pipeline downstream
            await _next.Invoke(context);

            // Log response
            await LogResponse(context.Response);
        }

        private async Task LogRequest(HttpRequest request)
        {
            // First log basic request info
            var requestInfo = GetRequestBasicInfo(request);
            _logger.Info("Start handling request: {0}", requestInfo);

            // Then log details about the request
            var requestDetails = GetRequestDetailsInfo(request);
            _logger.Debug("[Request Details][{0}]", requestDetails);

            // Then log request body
            // Note the original request body stream is write only, so use the below logic
            // to swap out a read/write memory stream with rewinding.
            using (var bodyReader = new StreamReader(request.Body))
            {
                string body = await bodyReader.ReadToEndAsync();

                request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
                _logger.Debug("[Request Body][{0}]", body);
            }
        }

        private async Task LogResponse(HttpResponse response)
        {
            // Log basic request info
            var requestInfo = GetRequestBasicInfo(response.HttpContext.Request);
            _logger.Info("Finished handling request: {0}", requestInfo);

            // Then log details about the response
            var responseDetails = GetResponseDetailsInfo(response);
            _logger.Debug("[Response Details][{0}]", responseDetails);

            // We don't log response body for now as that produces too much noise and 
            // is not expected to be as helpful for request debugging
        }

        private static string GetRequestBasicInfo(HttpRequest request)
        {
            return string.Format("{0} {1}", request.Method, request.Path);
        }

        private static string GetRequestDetailsInfo(HttpRequest request)
        {
            return string.Format("Client IP: {0}, Path: {1}, Content Type: {2}, Content Length: {3}, Headers: {4}",
                request.HttpContext.Connection.RemoteIpAddress,
                request.Path,
                request.ContentType,
                request.ContentLength,
                request.Headers.ToJson()
            );
        }

        private static string GetResponseDetailsInfo(HttpResponse response)
        {
            return string.Format("Status Code: {0}, Content Type: {1}, Content Length: {2}, Headers: {3}",
                response.StatusCode,
                response.ContentType,
                response.ContentLength,
                response.Headers.ToJson()
            );
        }
    }
}
