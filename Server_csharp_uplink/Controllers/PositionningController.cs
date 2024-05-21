using Microsoft.AspNetCore.Mvc;

namespace Server_csharp_uplink.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PositionningController : ControllerBase
    {
        private readonly ILogger<PositionningController> _logger;

        public PositionningController(ILogger<PositionningController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "PositionSensor")]
        public String Post()
        {
            return "Hello World\n";
        }

        [HttpGet(Name = "Start")]
        public String Get()
        {
            return "Hello World\n";
        }
    }
}
