using System.Collections.Generic;
using System.Threading.Tasks;
using MyWebService.Data.ElasticSearch;
using Microsoft.AspNetCore.Mvc;

namespace MyWebService.Controllers.Api
{
    public class ApiWithEsControllerBase<TEntity> : Controller where TEntity : class
    {
        protected IEsRepository EsRepository { get; private set; }

        public ApiWithEsControllerBase(IEsRepository esRepository)
        {
            EsRepository = esRepository;
        }

        [Route("~/api/v1/[controller]/raw/{rawQuery}"), HttpGet]
        public async Task<IEnumerable<TEntity>> GetByRawQuery(string rawQuery)
        {
            // Example of using the query builder
            return await EsRepository.SearchEntitiesAsyncWithQuery<TEntity>(q => q.Raw(rawQuery));
        }
    }
}
