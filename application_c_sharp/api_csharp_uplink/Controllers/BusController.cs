using Microsoft.AspNetCore.Mvc;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Dto;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Controllers
{
    /// <summary>
    /// Controller to manage DevEuiCard.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BusController(IBusService busService) : ControllerBase
    {
        private readonly IBusService _busService = busService;

        /// <summary>
        /// Register a new card depending on busNumber and his DevEUI code
        /// </summary>
        /// <param name="busDto">The schema bus DTO with busNumber, LineNumber and DevEuiCard</param>
        /// <returns>A Bus created if success or a conflict status if the bus is already created</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Bus), 201)]
        [ProducesResponseType(409)]
        public IActionResult AddBusCard([FromBody] BusDto busDto)
        {
            try
            {
                return Created($"api/bus/busNumber/{busDto.BusNumber}", _busService.CreateBus(busDto.LineBus, busDto.BusNumber, busDto.DevEuiCard));
            }
            catch(Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }

        /// <summary>
        /// Get a bus depending on busNumber
        /// </summary>
        /// <param name="busNumber">The bus number</param>
        /// <returns>A response with bus or a NotFound response</returns>
        [HttpGet("busNumber/{busNumber}")]
        [ProducesResponseType(typeof(Bus), 200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Get a bus depending on DevEUI number
        /// </summary>
        /// <param name="devEuiCard">The devEUI of the card</param>
        /// <returns>A response with bus or a NotFound response</returns>
        [HttpGet("devEuiCard/{devEuiCard}")]
        [ProducesResponseType(typeof(Bus), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetBusByDevEui(string devEuiCard)
        {
            try
            {
                return Ok(_busService.GetBusByDevEuiCard(devEuiCard));
            }
            catch(Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }

        /// <summary>
        /// Get all buses
        /// </summary>
        /// <returns>A list of buses</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Bus>), 200)]
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
