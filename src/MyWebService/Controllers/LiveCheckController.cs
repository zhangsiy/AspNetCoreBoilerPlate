using System.Reflection;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyWebService.Controllers
{
    /// <summary>
    /// A end point to allow external pings to detect the up status of the service
    /// </summary>
    [Route("/[controller]")]
    public class LiveCheckController : Controller
    {
        /// <summary>
        /// Ping to get a live check response
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Get()
        {
            return new
            {
                Message = "The Service Is Live!",
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(4),
            };
        }

    }
}
