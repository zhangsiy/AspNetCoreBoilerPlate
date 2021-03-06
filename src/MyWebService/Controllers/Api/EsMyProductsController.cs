﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebService.Controllers.Api.Common.ResponseBuilder;
using MyWebService.Data.ElasticSearch;
using MyWebService.Models.MyProduct;

namespace MyWebService.Controllers.Api
{
    /// <summary>
    /// Sample controller with Elastic Search support
    /// </summary>
    [Route("api/v1/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class EsMyProductsController: ApiWithEsControllerBase<MyProduct>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="esRepository">The (injected) Elastic Search repository instance</param>
        public EsMyProductsController(IEsRepository esRepository)
            : base(esRepository)
        {
        }

        /// <summary>
        /// Get all MyProduct entities.
        /// This performs an unbounded search on the index
        /// </summary>
        /// <returns>The list of entities</returns>
        [Route(""), HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Example of using the search request builder
            var products = await EsRepository.SearchEntitiesAsync<MyProduct>(s => s
                .Size(1000));
            return Json(products);
        }

        /// <summary>
        /// Get entities by ID
        /// This performs a term query on the index with the ID given
        /// </summary>
        /// <param name="myProductId">The ID to search for</param>
        /// <returns>The list of entities match the search</returns>
        [Route("{myProductId}"), HttpGet]
        public async Task<IActionResult> GetById(string myProductId)
        {
            // Example of using the query builder
            var result = await EsRepository.SearchEntitiesAsyncWithQuery<MyProduct>(q => q
                .Term("myProductId", myProductId));
            return Json(result);
        }
    }
}
