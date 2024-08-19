using api_csharp_uplink.Connectors.ExternalEntities;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp_uplink.Controllers;

public class TimeDistanceDto(int time, double distance)
{
    public int Time { get; init; } = time;
    public double Distance { get; init; } = distance;
}

[ApiController]
[Route("api/[controller]")]
public class TimeEngineController(IGraphHelper graphHelper, ITimeProcessor timeProcessor) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(TimeDistanceDto), 200)]
    public async Task<IActionResult> GetData(PositionDto[] positionDtos)
    {
        try
        {
            if (positionDtos.Length != 2)
            {
                return BadRequest();
            }
            
            Position position1 = new Position(positionDtos[0].Latitude, positionDtos[0].Longitude);
            Position position2 = new Position(positionDtos[1].Latitude, positionDtos[1].Longitude);
            TimeDistance timeDistance = await graphHelper.GetTimeAndDistance(position1, position2);
            return Ok(new TimeDistanceDto(timeDistance.Time, timeDistance.Distance));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Ok(false);
        }
    }
    
    [HttpGet("{lineNumber:int}/{station1}/{station2}")]
    [ProducesResponseType(typeof(int), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetTimeDistance(int lineNumber, string station1, string station2)
    {
        try
        {
            int time = await timeProcessor.GetTimeBetweenStations(lineNumber, station1, station2);
            return Ok(time);
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }
}