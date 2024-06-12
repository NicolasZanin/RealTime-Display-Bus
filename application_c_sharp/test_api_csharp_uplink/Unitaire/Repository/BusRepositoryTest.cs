using api_csharp_uplink.Dto;
using Moq;
using api_csharp_uplink.Repository;
using api_csharp_uplink.Entities;
using api_csharp_uplink.DB;

namespace test_api_csharp_uplink.Unitaire.Repository
{
    public class BusRepositoryTest
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
            Mock<IInfluxDbBus> mock = new();
            mock.SetupSequence(influxDbBus => influxDbBus.Add(It.IsAny<Bus>()))
                .ReturnsAsync(_busExpected)
                .ReturnsAsync(null as Bus);
            
            BusRepository busRepository = new(mock.Object);

            Bus? busActual = busRepository.AddBus(_busExpected);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);

            busActual = busRepository.AddBus(_busExpected);
            Assert.Null(busActual);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByNumber()
        {
            Mock<IInfluxDbBus> mock = new();
            mock.SetupSequence(influxDbBus => influxDbBus.GetByBusNumber(_busExpected.BusNumber))
                .ReturnsAsync(_busExpected)
                .ReturnsAsync(null as Bus);

            BusRepository busRepository = new(mock.Object);

            Bus? busActual = busRepository.GetByBusNumber(_busDto.BusNumber);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);

            busActual = busRepository.GetByBusNumber(_busDto.BusNumber);
            Assert.Null(busActual);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByDevEui()
        {
            Mock<IInfluxDbBus> mock = new();
            mock.SetupSequence(influxDbBus => influxDbBus.GetByDevEui(_busExpected.DevEuiCard))
                .ReturnsAsync(_busExpected)
                .ReturnsAsync(null as Bus);

            BusRepository busRepository = new(mock.Object);

            Bus? busActual = busRepository.GetBusByDevEuiCard(_busDto.DevEuiCard);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);

            busActual = busRepository.GetBusByDevEuiCard(_busDto.DevEuiCard);
            Assert.Null(busActual);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBuses()
        {
            Bus busExpected1 = new(1, "1", 1);

            Bus busExpected2 = new(2, "2", 5);

            Bus busExpected3 = new(3, "3", 5);
            Mock<IInfluxDbBus> mock = new();
            mock.SetupSequence(influxDbBus => influxDbBus.GetAll())
                .ReturnsAsync([])
                .ReturnsAsync([busExpected1])
                .ReturnsAsync([busExpected1, busExpected2, busExpected3]);

            BusRepository busRepository = new(mock.Object);
            List<Bus> buses = busRepository.GetBuses();

            Assert.Empty(buses);

            buses = busRepository.GetBuses();
            Assert.Equal(Assert.Single(buses), busExpected1);

            buses = busRepository.GetBuses();
            Assert.Equal(3, buses.Count);
            Assert.Equal([busExpected1, busExpected2, busExpected3], buses);
        }
    }
}
