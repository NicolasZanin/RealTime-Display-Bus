using Microsoft.AspNetCore.Mvc;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Dto;

namespace api_csharp_uplink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusController(IBusService busService) : ControllerBase
    {
        private readonly IBusService _busService = busService;

        [HttpPost()]
        public IActionResult AddBusCard([FromBody] BusDTO busDTO)
        {
            return Ok(_busService.CreateBus(busDTO.LineBus, busDTO.BusNumber,  busDTO.DevEUICard));
        }

        [HttpGet("busNumber/{busNumber}")]
        public IActionResult GetBusByBusNumber(int busNumber)
        {
            return Ok(_busService.GetBusByBusNumber(busNumber));
        }

        [HttpGet("DevEUI/{devEUI}")]
        public IActionResult GetBusByDeEUIv(int devEUI)
        {
            return Ok(_busService.GetBusByDevEUICard(devEUI));
        }

        [HttpGet("")]
        public IActionResult GetBuses()
        {
            return Ok(_busService.GetBuses());
        }
    }
}
