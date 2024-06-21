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
    public class BusControllerTest
    {

        private readonly BusDto _busDto = new()
        {
            LineBus = 1,
            BusNumber = 0,
            DevEuiCard = "0"
        };
        private readonly Bus _busExpected = new(0, "0", 1);

        [Fact]
        [Trait("Category", "Unit")]
        public void TestCreateBus()
        {


            Mock<ICardRepository> mock = new();
            mock.Setup(cardRepository => cardRepository.Add(_busExpected)).ReturnsAsync(_busExpected);
            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant, busComposant);

            IActionResult actionResult = busController.AddBusCard(_busDto);
            actionResult.Should().BeOfType<CreatedResult>();
            CreatedResult createdResult = (CreatedResult) actionResult;
            createdResult.Should().NotBeNull();
            createdResult.Value.Should().Be(_busExpected);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestFalseCreate2BusSameTime()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.Add(_busExpected))
                .ReturnsAsync(_busExpected)
                .ReturnsAsync(_busExpected);
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_busExpected.DevEuiCard))
                .ReturnsAsync(null as Bus)
                .ReturnsAsync(_busExpected);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant, busComposant);

            busController.AddBusCard(_busDto);
            IActionResult actionResult = busController.AddBusCard(_busDto);
            actionResult.Should().BeOfType<ConflictObjectResult>();
        }
        

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByDevEui()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_busExpected.DevEuiCard))
                .ReturnsAsync(_busExpected)
                .ReturnsAsync(null as Bus);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant, busComposant);

            IActionResult actionResult = busController.GetBusByDevEui(_busDto.DevEuiCard);
            actionResult.Should().BeOfType<OkObjectResult>();
            OkObjectResult? okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            okObject?.Value.Should().Be(_busExpected);

            actionResult = busController.GetBusByDevEui(_busDto.DevEuiCard);
            actionResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBuses()
        {
            Bus busExpected2 = new(2, "2", 5);

            Bus busExpected3 = new(3, "3", 5);
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetAll())
                .ReturnsAsync([])
                .ReturnsAsync([_busExpected])
                .ReturnsAsync([_busExpected, busExpected2, busExpected3]);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant, busComposant);

            IActionResult actionResult = busController.GetBuses();
            actionResult.Should().BeOfType<OkObjectResult>();
            OkObjectResult? okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            List<Bus>? buses = okObject?.Value as List<Bus>;
            buses.Should().BeEmpty();

            actionResult = busController.GetBuses();
            actionResult.Should().BeOfType<OkObjectResult>();
            okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            buses = okObject?.Value as List<Bus>;
            buses.Should().BeEquivalentTo([_busExpected]);

            actionResult = busController.GetBuses();
            actionResult.Should().BeOfType<OkObjectResult>();
            okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            buses = okObject?.Value as List<Bus>;
            buses.Should().BeEquivalentTo([_busExpected, busExpected2, busExpected3]);
        }
    }
}
