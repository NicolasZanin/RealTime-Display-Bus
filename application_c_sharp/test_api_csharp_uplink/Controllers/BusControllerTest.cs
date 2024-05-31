using api_csharp_uplink.Controllers;
using api_csharp_uplink.Dto;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq;
using api_csharp_uplink.Repository;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Entities;

namespace test_api_csharp_uplink.Controllers
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
        public void TestFalseCreateBus()
        {

        }
    }
}
