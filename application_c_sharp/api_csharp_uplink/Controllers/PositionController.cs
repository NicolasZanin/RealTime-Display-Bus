using api_csharp_uplink.DirException;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp_uplink.Controllers;

/// <summary>
/// Controller to manage position depending on DevEUI number.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PositionController(IPositionComposant positionComposant) : ControllerBase
{
    
    /// <summary>
    /// Register a new card depending on busNumber and his DevEUI code
    /// </summary>
    /// <param name="positionBusDto">The schema position of the Card with Longitude, Latitude and DevEuiCard</param>
    /// <returns>A position of the bus created</returns>
    /// <response code="200">Returns the position of the bus.</response>
    /// <response code="400">if the position of bus has not good value</response>
    /// <response code="500">If there is a server error.</response>
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
    
    /// <summary>
    /// Get a position of bus depending on DevEUI number
    /// </summary>
    /// <param name="devEuiCard">The devEui number of card</param>
    /// <returns>A response with position of bus</returns>
    /// <response code="200">Returns the position of the bus.</response>
    /// <response code="404">If a bus with the specified DevEUI number is not found.</response>
    /// <response code="500">If there is a server error.</response>
    [HttpGet("devEuiCard/{devEuiCard}")]
    [ProducesResponseType(typeof(PositionBusDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetLastPositionByDevEuiNumber(string devEuiCard)
    {
        try
        {
            PositionBus positionBus = positionComposant.GetLastPosition(devEuiCard);
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

    /**
     * Convert a PositionBus into a PositionBusDto
     * @param positionBus The positionBus to convert
     */
    private static PositionBusDto ConvertPositionBusIntoDto(PositionBus positionBus)
    {
        return new PositionBusDto
        {
            DevEuiNumber = positionBus.DevEuiCard,
            Position = new PositionDto
            {
                Latitude = positionBus.Position.Latitude,
                Longitude = positionBus.Position.Longitude
            }
        };
    }
}
