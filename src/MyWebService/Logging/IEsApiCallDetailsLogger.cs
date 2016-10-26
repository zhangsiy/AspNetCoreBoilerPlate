using System.Threading.Tasks;
using Elasticsearch.Net;

namespace MyWebService.Logging
{
    public interface IEsApiCallDetailsLogger
    {
        void LogEsApiCallDetails(IApiCallDetails esApiCallDetails);

        Task LogEsApiCallDetailsAsync(IApiCallDetails esApiCallDetails);
    }
}
