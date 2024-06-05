using Microsoft.AspNetCore.Mvc;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Dto;
using api_csharp_uplink.DirException;

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
            try
            {
                return Created($"api/bus/busNumber/{busDTO.BusNumber}", _busService.CreateBus(busDTO.LineBus, busDTO.BusNumber, busDTO.DevEUICard));
            }
            catch(Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }

        [HttpGet("busNumber/{busNumber}")]
        public IActionResult GetBusByBusNumber(int busNumber)
        {
            try
            {
                return Ok(_busService.GetBusByBusNumber(busNumber));
            }
            catch(Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }

        [HttpGet("DevEUI/{devEUI}")]
        public IActionResult GetBusByDeEUIv(int devEUI)
        {
            try
            {
                return Ok(_busService.GetBusByDevEUICard(devEUI));
            }
            catch(Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }

        [HttpGet()]
        public IActionResult GetBuses()
        {
            try
            {
                return Ok(_busService.GetBuses());
            }
            catch(Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }
    }
}
