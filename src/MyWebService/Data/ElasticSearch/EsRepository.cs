using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyWebService.Logging;
using Elasticsearch.Net;
using Elasticsearch.Net.Aws;
using Microsoft.Extensions.Options;
using MyWebService.Models.MyProduct;
using Nest;
using NLog;

namespace MyWebService.Data.ElasticSearch
{
    internal class EsRepository : IEsRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly IEsApiCallDetailsLogger EsApiLog = new EsApiCallDetailsLogger();

        private IElasticClient Client { get; set; }

        public EsRepository(IOptions<EsRepoConfiguration> esConfiguration)
        {
            var esConfig = esConfiguration.Value;

            var httpConnection = new AwsHttpConnection(new AwsSettings
            {
                AccessKey = esConfig.AwsConfiguration.AccessKey,
                SecretKey = esConfig.AwsConfiguration.SecretKey,
                Region = esConfig.AwsConfiguration.Region
            });

            var pool = new SingleNodeConnectionPool(new Uri(esConfig.ServerUrl));

            // Configure the connection
            //  - Also hookup asyn logging of all API call details (EnableTrace not avaialble in NEST 2.x, yet.)
            var config = new ConnectionSettings(pool, httpConnection)
                .DisableDirectStreaming()
                .OnRequestCompleted(r => EsApiLog.LogEsApiCallDetailsAsync(r));

            // Default index mappings to types...
            // To bad there is no attribute based mapping for this
            // [TO_FILL register ElasticSearch index mapping here]
            config.MapDefaultTypeIndices(m => m
                .Add(typeof(MyProduct), "my_product_index")
            );

            // Create the client
            Client = new ElasticClient(config);
        }


        public async Task<IEnumerable<T>> SearchEntitiesAsync<T>(Func<SearchDescriptor<T>, ISearchRequest> buildRequest) where T : class
        {
            var response = await Client.SearchAsync<T>(buildRequest);
            return response.Documents;
        }

        public async Task<IEnumerable<T>> SearchEntitiesAsyncWithQuery<T>(Func<QueryContainerDescriptor<T>, QueryContainer> buildQuery) where T : class
        {
            var response = await Client.SearchAsync<T>(s => s
                .Size(EsConstants.DefaultSearchResultNumPerBatch)
                .Query(buildQuery));
            return response.Documents;
        }
    }
}
