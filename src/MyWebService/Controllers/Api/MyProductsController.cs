using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebService.Models.MyProduct;
using NLog;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyWebService.Controllers.Api
{
    [Route("api/v1/[controller]")]
    [ResponseCache(CacheProfileName = "Default")]
    public class MyProductsController : Controller
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private List<MyProduct> AllMyProducts { get; set; } 
        public MyProductsController()
        {
            AllMyProducts = new List<MyProduct> {
                new MyProduct { MyProductId = "AAA-001", Name = "My Product 1", Description = "First test MyProduct"},
                new MyProduct { MyProductId = "AAA-002", Name = "My Product 2", Description = "Second test MyProduct" },
                new MyProduct { MyProductId = "AAA-003", Name = "My Product 3", Description = "Third test MyProduct" }
            };
        }

        [Route(""), HttpGet]
        public async Task<IEnumerable<MyProduct>> GetAll()
        {
            // Example of using the search request builder
            return AllMyProducts;
        }

        [Route("{myProductId}"), HttpGet]
        public async Task<MyProduct> GetById(string myProductId)
        {
            // Example of using the query builder
            return AllMyProducts.FirstOrDefault(p => p.MyProductId == myProductId);
        }
    }
}
