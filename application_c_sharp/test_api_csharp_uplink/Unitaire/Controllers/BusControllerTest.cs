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
        [Fact]
        [Trait("Category", "Unit")]
        public void TestCreateBus()
        {
            BusDTO bus = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };
            Bus busExpected = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };

            Mock<IBusRepository> mock = new();
            mock.Setup(mock => mock.AddBus(busExpected)).Returns(busExpected);
            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            IActionResult actionResult = busController.AddBusCard(bus);
            actionResult.Should().BeOfType<CreatedResult>();
            CreatedResult createdResult = (CreatedResult) actionResult;
            createdResult.Should().NotBeNull();
            createdResult.Value.Should().Be(busExpected);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestFalseCreate2BusSameTime()
        {
            BusDTO bus = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };
            Bus busExpected = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };

            Mock<IBusRepository> mock = new();
            mock.SetupSequence(mock => mock.AddBus(busExpected))
                .Returns(busExpected)
                .Returns(busExpected);
            mock.SetupSequence(mock => mock.GetByBusNumber(busExpected.BusNumber))
                .Returns(null as Bus)
                .Returns(busExpected);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            busController.AddBusCard(bus);
            IActionResult actionResult = busController.AddBusCard(bus);
            actionResult.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByNumber()
        {
            BusDTO bus = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };

            Bus busExpected = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };
            Mock<IBusRepository> mock = new();
            mock.SetupSequence(mock => mock.GetByBusNumber(busExpected.BusNumber))
                .Returns(busExpected)
                .Returns(null as Bus);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            IActionResult actionResult = busController.GetBusByBusNumber(bus.BusNumber);
            actionResult.Should().BeOfType<OkObjectResult>();
            OkObjectResult? okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            okObject?.Value.Should().Be(busExpected);

            actionResult = busController.GetBusByBusNumber(bus.BusNumber);
            actionResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByDevEUI()
        {
            BusDTO bus = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };

            Bus busExpected = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };
            Mock<IBusRepository> mock = new();
            mock.SetupSequence(mock => mock.GetBusByDevEUICard(busExpected.BusNumber))
                .Returns(busExpected)
                .Returns(null as Bus);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            IActionResult actionResult = busController.GetBusByDeEUIv(bus.BusNumber);
            actionResult.Should().BeOfType<OkObjectResult>();
            OkObjectResult? okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            okObject?.Value.Should().Be(busExpected);

            actionResult = busController.GetBusByDeEUIv(bus.BusNumber);
            actionResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBuses()
        {
            BusDTO bus = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };

            Bus busExpected1 = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
            };

            Bus busExpected2 = new()
            {
                LineBus = 5,
                BusNumber = 2,
                DevEUICard = 2
            };

            Bus busExpected3 = new()
            {
                LineBus = 5,
                BusNumber = 3,
                DevEUICard = 3
            };
            Mock<IBusRepository> mock = new();
            mock.SetupSequence(mock => mock.GetBuses())
                .Returns([])
                .Returns([busExpected1])
                .Returns([busExpected1, busExpected2, busExpected3]);

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
            buses.Should().BeEquivalentTo([busExpected1]);

            actionResult = busController.GetBuses();
            actionResult.Should().BeOfType<OkObjectResult>();
            okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            buses = okObject?.Value as List<Bus>;
            buses.Should().BeEquivalentTo([busExpected1, busExpected2, busExpected3]);
        }
    }
}
