using api_csharp_uplink.Controllers;
using api_csharp_uplink.Dto;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Controllers;

public class CardControllerTest
{

    private readonly CardDto _cardDto = new()
    {
        LineBus = 1,
        DevEuiCard = "0"
    };
    private readonly Card _cardExpected = new("0", 1);
    private readonly CardController _cardController;
    
    public CardControllerTest()
    {
        ICardRepository cardRepository = new DbTestCard();
        CardComposant cardComposant = new(cardRepository);
        _cardController = new(cardComposant, cardComposant);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestCreateCard()
    {
        IActionResult actionResult = _cardController.AddCard(_cardDto);
        actionResult.Should().BeOfType<CreatedResult>();
        CreatedResult createdResult = (CreatedResult) actionResult;
        createdResult.Should().NotBeNull();
        createdResult.Value.Should().Be(_cardExpected);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestFalseCreate2CardSameTime()
    {
        _cardController.AddCard(_cardDto);
        IActionResult actionResult = _cardController.AddCard(_cardDto);
        actionResult.Should().BeOfType<ConflictObjectResult>();
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestModifyCard()
    {
        _cardController.AddCard(_cardDto);
        IActionResult actionResult = _cardController.ModifyCard(_cardDto);
        actionResult.Should().BeOfType<OkObjectResult>();
        OkObjectResult okResult = (OkObjectResult) actionResult;
        okResult.Should().NotBeNull();
        okResult.Value?.Should().Be(_cardExpected);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestModifyCardError()
    {
        IActionResult actionResult = _cardController.ModifyCard(_cardDto);
        actionResult.Should().BeOfType<NotFoundObjectResult>();
    }
    

    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetCardByDevEui()
    {
        IActionResult actionResult = _cardController.GetCardByDevEui(_cardDto.DevEuiCard);
        actionResult.Should().BeOfType<NotFoundObjectResult>();
        
        _cardController.AddCard(_cardDto);
        actionResult = _cardController.GetCardByDevEui(_cardDto.DevEuiCard);
        actionResult.Should().BeOfType<OkObjectResult>();
        OkObjectResult? okObject = actionResult as OkObjectResult;
        okObject.Should().NotBeNull();
        okObject?.Value.Should().Be(_cardExpected);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetCards()
    {
        Card cardExpected2 = new("2", 5);

        Card cardExpected3 = new("3", 5);

        IActionResult actionResult = _cardController.GetCards();
        actionResult.Should().BeOfType<OkObjectResult>();
        OkObjectResult? okObject = actionResult as OkObjectResult;
        okObject.Should().NotBeNull();
        List<Card>? buses = okObject?.Value as List<Card>;
        buses.Should().BeEmpty();

        _cardController.AddCard(_cardDto);
        actionResult = _cardController.GetCards();
        actionResult.Should().BeOfType<OkObjectResult>();
        okObject = actionResult as OkObjectResult;
        okObject.Should().NotBeNull();
        buses = okObject?.Value as List<Card>;
        buses.Should().BeEquivalentTo([_cardExpected]);

        _cardController.AddCard(new CardDto{DevEuiCard = cardExpected2.DevEuiCard, LineBus = cardExpected2.LineBus});
        _cardController.AddCard(new CardDto{DevEuiCard = cardExpected3.DevEuiCard, LineBus = cardExpected3.LineBus});
        actionResult = _cardController.GetCards();
        actionResult.Should().BeOfType<OkObjectResult>();
        okObject = actionResult as OkObjectResult;
        okObject.Should().NotBeNull();
        buses = okObject?.Value as List<Card>;
        buses.Should().BeEquivalentTo([_cardExpected, cardExpected2, cardExpected3]);
    }
}
