using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tag.Common.Data;

namespace Tag.Consumer.Controllers
{
    public class BaseAuthController : Controller
    {
        private DataContext db { get; set; }
        private ILogger<BaseAuthController> log { get; set; }
        public BaseAuthController(ILogger<BaseAuthController> logger, DataContext context)
        {
            db = context;
            log = logger;
        }
    }
}