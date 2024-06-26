using api_csharp_uplink.Controllers;
using api_csharp_uplink.Dto;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.Controllers
{
    public class CardControllerTest
    {

        private readonly CardDto _cardDto = new()
        {
            LineBus = 1,
            DevEuiCard = "0"
        };
        private readonly Card _cardExpected = new("0", 1);

        [Fact]
        [Trait("Category", "Unit")]
        public void TestCreateBus()
        {


            Mock<ICardRepository> mock = new();
            mock.Setup(cardRepository => cardRepository.Add(_cardExpected)).Returns(_cardExpected);
            CardComposant cardComposant = new(mock.Object);
            CardController cardController = new(cardComposant, cardComposant);

            IActionResult actionResult = cardController.AddBusCard(_cardDto);
            actionResult.Should().BeOfType<CreatedResult>();
            CreatedResult createdResult = (CreatedResult) actionResult;
            createdResult.Should().NotBeNull();
            createdResult.Value.Should().Be(_cardExpected);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestFalseCreate2BusSameTime()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.Add(_cardExpected))
                .Returns(_cardExpected)
                .Returns(_cardExpected);
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_cardExpected.DevEuiCard))
                .Returns(null as Card)
                .Returns(_cardExpected);

            CardComposant cardComposant = new(mock.Object);
            CardController cardController = new(cardComposant, cardComposant);

            cardController.AddBusCard(_cardDto);
            IActionResult actionResult = cardController.AddBusCard(_cardDto);
            actionResult.Should().BeOfType<ConflictObjectResult>();
        }
        

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByDevEui()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_cardExpected.DevEuiCard))
                .Returns(_cardExpected)
                .Returns(null as Card);

            CardComposant cardComposant = new(mock.Object);
            CardController cardController = new(cardComposant, cardComposant);

            IActionResult actionResult = cardController.GetBusByDevEui(_cardDto.DevEuiCard);
            actionResult.Should().BeOfType<OkObjectResult>();
            OkObjectResult? okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            okObject?.Value.Should().Be(_cardExpected);

            actionResult = cardController.GetBusByDevEui(_cardDto.DevEuiCard);
            actionResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBuses()
        {
            Card cardExpected2 = new("2", 5);

            Card cardExpected3 = new("3", 5);
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetAll())
                .Returns([])
                .Returns([_cardExpected])
                .Returns([_cardExpected, cardExpected2, cardExpected3]);

            CardComposant cardComposant = new(mock.Object);
            CardController cardController = new(cardComposant, cardComposant);

            IActionResult actionResult = cardController.GetBuses();
            actionResult.Should().BeOfType<OkObjectResult>();
            OkObjectResult? okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            List<Card>? buses = okObject?.Value as List<Card>;
            buses.Should().BeEmpty();

            actionResult = cardController.GetBuses();
            actionResult.Should().BeOfType<OkObjectResult>();
            okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            buses = okObject?.Value as List<Card>;
            buses.Should().BeEquivalentTo([_cardExpected]);

            actionResult = cardController.GetBuses();
            actionResult.Should().BeOfType<OkObjectResult>();
            okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            buses = okObject?.Value as List<Card>;
            buses.Should().BeEquivalentTo([_cardExpected, cardExpected2, cardExpected3]);
        }
    }
}
