
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TagConsumer.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
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