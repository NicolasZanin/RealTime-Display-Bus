using Microsoft.AspNetCore.Mvc;

namespace Server_csharp_uplink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionningController : ControllerBase
    {
        private readonly ILogger<PositionningController> _logger;

        public PositionningController(ILogger<PositionningController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IActionResult timeBusToNextStation([FromQuery] int idStation)
        {
            if (idStation < 0)
            {
                return BadRequest("idStation must be a non-negative integer");
            }

            return Ok("5 mn");
        }
    }
}
