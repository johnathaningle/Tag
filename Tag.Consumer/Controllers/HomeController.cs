
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TagConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public string Index()
        {
            return "Hello World";
        }

        [HttpPost]
        public JsonResult CreateDirectoryStructure()
        {
            return new JsonResult(new {FileStatus = "Created"});
        }
    }
}