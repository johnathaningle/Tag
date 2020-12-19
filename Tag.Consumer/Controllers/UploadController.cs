using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tag.Common.Data;

namespace Tag.Consumer.Controllers
{
    public class UploadController : BaseAuthController
    {
        public UploadController(ILogger<UploadController> logger, DataContext context) : base(logger, context)
        {
        }

        
    }
}