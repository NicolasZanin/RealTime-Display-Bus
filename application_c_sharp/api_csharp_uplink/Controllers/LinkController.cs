using api_csharp_uplink.DirException;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp_uplink.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinkController(IAddLink addLink, ILinkFinder linkFinder) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(LinkDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddLink(LinkDto link)
    {
        try
        {
            Link linkAdd =
                await addLink.AddLink(link.NameStation1, link.NameStation2, link.LineNumber, link.Orientation);
            return Created($"", ConvertLinkToLinkDto(linkAdd));
        }
        catch (Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }

    [HttpGet("{nameStation1}/{nameStation2}/{lineNumber:int}")]
    [ProducesResponseType(typeof(LinkDto), 200)]
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
    [ProducesResponseType(typeof(LinkDto), 200)]
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
    [ProducesResponseType(typeof(LinkDto), 200)]
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


    private static LinkDto ConvertLinkToLinkDto(Link link)
    {
        return new LinkDto
        {
            NameStation1 = link.nameStation1,
            NameStation2 = link.nameStation2,
            LineNumber = link.lineNumber,
            Orientation = link.orientation.ToString(),
        };
    }
}