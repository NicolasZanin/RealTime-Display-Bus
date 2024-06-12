using api_csharp_uplink.Controllers;
using api_csharp_uplink.Dto;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using api_csharp_uplink.Repository;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Entities;

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


            Mock<IBusRepository> mock = new();
            mock.Setup(busRepository => busRepository.AddBus(_busExpected)).Returns(_busExpected);
            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

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
            Mock<IBusRepository> mock = new();
            mock.SetupSequence(busRepository => busRepository.AddBus(_busExpected))
                .Returns(_busExpected)
                .Returns(_busExpected);
            mock.SetupSequence(busRepository => busRepository.GetByBusNumber(_busExpected.BusNumber))
                .Returns(null as Bus)
                .Returns(_busExpected);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            busController.AddBusCard(_busDto);
            IActionResult actionResult = busController.AddBusCard(_busDto);
            actionResult.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByNumber()
        {
            Mock<IBusRepository> mock = new();
            mock.SetupSequence(busRepository => busRepository.GetByBusNumber(_busExpected.BusNumber))
                .Returns(_busExpected)
                .Returns(null as Bus);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            IActionResult actionResult = busController.GetBusByBusNumber(_busDto.BusNumber);
            actionResult.Should().BeOfType<OkObjectResult>();
            OkObjectResult? okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            okObject?.Value.Should().Be(_busExpected);

            actionResult = busController.GetBusByBusNumber(_busDto.BusNumber);
            actionResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByDevEui()
        {
            Mock<IBusRepository> mock = new();
            mock.SetupSequence(busRepository => busRepository.GetBusByDevEuiCard(_busExpected.DevEuiCard))
                .Returns(_busExpected)
                .Returns(null as Bus);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

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
            Mock<IBusRepository> mock = new();
            mock.SetupSequence(busRepository => busRepository.GetBuses())
                .Returns([])
                .Returns([_busExpected])
                .Returns([_busExpected, busExpected2, busExpected3]);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

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
