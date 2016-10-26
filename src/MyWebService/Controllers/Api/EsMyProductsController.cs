using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebService.Data.ElasticSearch;
using MyWebService.Models.MyProduct;
using NLog;

namespace MyWebService.Controllers.Api
{
    [Route("api/v1/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class EsMyProductsController: ApiWithEsControllerBase<MyProduct>
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public EsMyProductsController(IEsRepository esRepository)
            : base(esRepository)
        {
        }

        [Route(""), HttpGet]
        public async Task<IEnumerable<MyProduct>> GetAll()
        {
            // Example of using the search request builder
            return await EsRepository.SearchEntitiesAsync<MyProduct>(s => s
                .Size(1000));
        }

        [Route("{myProductId}"), HttpGet]
        public async Task<IEnumerable<MyProduct>> GetById(string myProductId)
        {
            // Example of using the query builder
            return await EsRepository.SearchEntitiesAsyncWithQuery<MyProduct>(q => q
                .Term("myProductId", myProductId));
        }
    }
}
