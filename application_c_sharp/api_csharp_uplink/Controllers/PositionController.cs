using api_csharp_uplink.DirException;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp_uplink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController(IPositionComposant positionComposant) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(PositionBusDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult AddNewPosition([FromBody] PositionBusDto positionBusDto)
        {
            try
            {
               PositionBus positionBus = positionComposant.AddPosition(positionBusDto.Position.Latitude,
                    positionBusDto.Position.Longitude,
                    positionBusDto.DevEuiNumber);
                
                return Created($"api/devEuiNumber/{positionBusDto.DevEuiNumber}", ConvertPositionBusIntoDto(positionBus));
            }
            catch (Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }
        
        [HttpGet("devEuiNumber/{devEuiNumber}")]
        [ProducesResponseType(typeof(PositionBusDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetLastPositionByDevEuiNumber(int devEuiNumber)
        {
            try
            {
                PositionBus positionBus = positionComposant.GetLastPosition(devEuiNumber);
                return Ok(ConvertPositionBusIntoDto(positionBus));
            }
            catch (Exception e)
            {
                return ErrorManager.HandleError(e);
            }
        }
        
        

        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult TimeBusToNextStation([FromQuery] int idStation)
        {
            if (idStation < 0)
            {
                return BadRequest("idStation must be a non-negative integer");
            }

            return Ok("5 mn");
        }

        private static PositionBusDto ConvertPositionBusIntoDto(PositionBus positionBus)
        {
            return new PositionBusDto
            {
                DevEuiNumber = positionBus.DevEuiNumber,
                Position = new PositionDto
                {
                    Latitude = positionBus.Position.Latitude,
                    Longitude = positionBus.Position.Longitude
                }
            };
        }
    }
}
