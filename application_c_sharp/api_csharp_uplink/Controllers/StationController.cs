using api_csharp_uplink.DirException;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp_uplink.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StationController(IStationComposant stationComposant) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(StationDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public IActionResult AddStation([FromBody] StationDto stationDto)
    {
        try
        {
           Station station = stationComposant.AddStation(stationDto.Position.Latitude,
                stationDto.Position.Longitude,
                stationDto.NameStation);
            
            return Created($"api/Station?nameStation={stationDto.NameStation}", ConvertStationIntoDto(station));
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(StationDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetStationByName([FromQuery] string nameStation)
    {
        try
        {
            Station station = stationComposant.GetStation(nameStation);
            return Ok(ConvertStationIntoDto(station));
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }
    
    [HttpGet("{latitude:double}/{longitude:double}")]
    [ProducesResponseType(typeof(StationDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public IActionResult GetStationByPosition(double latitude, double longitude)
    {
        try
        {
            Station station = stationComposant.GetStation(latitude, longitude);
            return Ok(ConvertStationIntoDto(station));
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }

    /**
     * Convert a Station into a StationDto
     * @param station The Station to convert
     */
    private static StationDto ConvertStationIntoDto(Station station)
    {
        return new StationDto
        {
            Position = new PositionDto
            {
                Latitude = station.Position.Latitude,
                Longitude = station.Position.Longitude
            },
            NameStation = station.NameStation
        };
    }
}