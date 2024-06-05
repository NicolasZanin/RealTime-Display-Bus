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

        private readonly BusDTO _busDTO;
        private readonly Bus _busExpected;

        public BusControllerTest()
        {
            _busDTO = new()
            {
                LineBus = 1,
                BusNumber = 0,
                DevEUICard = 0
            };
            _busExpected = new()
            {
                LineBus = 1,
                BusNumber = 0,
                DevEUICard = 0
            };
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestCreateBus()
        {


            Mock<IBusRepository> mock = new();
            mock.Setup(mock => mock.AddBus(_busExpected)).Returns(_busExpected);
            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            IActionResult actionResult = busController.AddBusCard(_busDTO);
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
            mock.SetupSequence(mock => mock.AddBus(_busExpected))
                .Returns(_busExpected)
                .Returns(_busExpected);
            mock.SetupSequence(mock => mock.GetByBusNumber(_busExpected.BusNumber))
                .Returns(null as Bus)
                .Returns(_busExpected);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            busController.AddBusCard(_busDTO);
            IActionResult actionResult = busController.AddBusCard(_busDTO);
            actionResult.Should().BeOfType<ConflictObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByNumber()
        {
            Mock<IBusRepository> mock = new();
            mock.SetupSequence(mock => mock.GetByBusNumber(_busExpected.BusNumber))
                .Returns(_busExpected)
                .Returns(null as Bus);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            IActionResult actionResult = busController.GetBusByBusNumber(_busDTO.BusNumber);
            actionResult.Should().BeOfType<OkObjectResult>();
            OkObjectResult? okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            okObject?.Value.Should().Be(_busExpected);

            actionResult = busController.GetBusByBusNumber(_busDTO.BusNumber);
            actionResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByDevEUI()
        {
            Mock<IBusRepository> mock = new();
            mock.SetupSequence(mock => mock.GetBusByDevEUICard(_busExpected.BusNumber))
                .Returns(_busExpected)
                .Returns(null as Bus);

            BusComposant busComposant = new(mock.Object);
            BusController busController = new(busComposant);

            IActionResult actionResult = busController.GetBusByDeEUIv(_busDTO.BusNumber);
            actionResult.Should().BeOfType<OkObjectResult>();
            OkObjectResult? okObject = actionResult as OkObjectResult;
            okObject.Should().NotBeNull();
            okObject?.Value.Should().Be(_busExpected);

            actionResult = busController.GetBusByDeEUIv(_busDTO.BusNumber);
            actionResult.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBuses()
        {
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
