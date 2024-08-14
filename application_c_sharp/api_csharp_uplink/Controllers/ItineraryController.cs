using api_csharp_uplink.DirException;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp_uplink.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItineraryController(IItineraryRegister itineraryRegister, IItineraryFinder itineraryFinder) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ItineraryDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddItineraire(ItineraryDto itineraryDto)
    {
        try
        {
            // TODO Convert Itinerary
            Itinerary itinerary =
                await itineraryRegister.AddItinerary(itineraryDto.LineNumber, itineraryDto.Orientation, null);
            return Created($"", null);
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }
    /*[HttpPost]
    [ProducesResponseType(typeof(ConnexionBeetweenStationDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddLink(ConnexionBeetweenStationDto connexionBeetweenStation)
    {
        try
        {
            Link linkAdd =
                await addItinerary.AddLink(connexionBeetweenStation.NameStation1, connexionBeetweenStation.NameStation2, connexionBeetweenStation.LineNumber, connexionBeetweenStation.Orientation);
            return Created($"", ConvertLinkToLinkDto(linkAdd));
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }

    [HttpGet("{nameStation1}/{nameStation2}/{lineNumber:int}")]
    [ProducesResponseType(typeof(ConnexionBeetweenStationDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> FindLink(string nameStation1, string nameStation2, int lineNumber)
    {
        try
        {
            Link link = await linkFinder.FindLink(nameStation1, nameStation2, lineNumber);
            return Ok(ConvertLinkToLinkDto(link));
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }

    [HttpGet("{lineNumber:int}")]
    [ProducesResponseType(typeof(ConnexionBeetweenStationDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> FindLinksByLineNumber(int lineNumber)
    {
        try
        {
            List<Link> links = await linkFinder.FindLinksByLineNumber(lineNumber);
            return Ok(links.Select(ConvertLinkToLinkDto));
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ConnexionBeetweenStationDto), 200)]
    public async Task<IActionResult> FindAllLinks()
    {
        try
        {
            List<Link> links = await linkFinder.FindAllLinks();
            return Ok(links.Select(ConvertLinkToLinkDto));
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }


    private static ConnexionBeetweenStationDto ConvertLinkToLinkDto(Link link)
    {
        return new ConnexionBeetweenStationDto
        {
            NameStation1 = link.nameStation1,
            NameStation2 = link.nameStation2,
            LineNumber = link.lineNumber,
            Orientation = link.orientation.ToString(),
        };
    }*/
}