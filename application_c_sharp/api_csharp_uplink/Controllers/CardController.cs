using Microsoft.AspNetCore.Mvc;
using api_csharp_uplink.Dto;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Controllers;

/// <summary>
/// Controller to manage Card.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CardController(ICardFinder cardFinder, ICardRegistration cardRegistration) : ControllerBase
{

    /// <summary>
    /// Register a new card depending on busNumber and his DevEUI code
    /// </summary>
    /// <param name="cardDto">The schema card DTO with LineNumber and DevEuiCard</param>
    /// <returns>A Card created if success or a conflict status if the card is already created</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Card), 201)]
    [ProducesResponseType(409)]
    public IActionResult AddBusCard([FromBody] CardDto cardDto)
    {
        try
        {
            return Created($"api/Card/devEuiCard/{cardDto.DevEuiCard}", cardRegistration.CreateCard(cardDto.LineBus, cardDto.DevEuiCard));
        }
        catch(Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }

    /// <summary>
    /// Get a card depending on DevEUI number
    /// </summary>
    /// <param name="devEuiCard">The devEUI of the card</param>
    /// <returns>A response with card or a NotFound response</returns>
    [HttpGet("devEuiCard/{devEuiCard}")]
    [ProducesResponseType(typeof(Card), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetBusByDevEui(string devEuiCard)
    {
        try
        {
            return Ok(cardFinder.GetCardByDevEuiCard(devEuiCard));
        }
        catch(Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }

    /// <summary>
    /// Get all cards
    /// </summary>
    /// <returns>A list of card</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Card>), 200)]
    public IActionResult GetBuses()
    {
        try
        {
            return Ok(cardFinder.GetCards());
        }
        catch(Exception e)
        {
            return ErrorManager.HandleError(e);
        }
    }
}
