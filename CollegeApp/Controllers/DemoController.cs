using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        //private readonly IMyLogger _myLogger;
        private readonly ILogger<DemoController> _logger;

        //// 1. Tightly-coupled

        //public DemoController()
        //{
        //    _myLogger = new LogToDB();
        //}


        //// 2. Loosely-coupled
        //public DemoController(IMyLogger myLogger) 
        //{
        //    _myLogger = myLogger;
        //}


        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public ActionResult Index()
        {
            _logger.LogTrace("Log message from trace method");
            _logger.LogDebug("Log message from debug method");
            _logger.LogInformation("Log message from information method");
            _logger.LogWarning("Log message from warning method");
            _logger.LogError("Log message from error method");
            _logger.LogCritical("Log message from critical method");

            return Ok();
        }

    }
}
