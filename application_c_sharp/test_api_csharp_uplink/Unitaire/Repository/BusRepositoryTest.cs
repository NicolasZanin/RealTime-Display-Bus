using Moq;
using api_csharp_uplink.Entities;
using api_csharp_uplink.DB;

namespace test_api_csharp_uplink.Unitaire.Repository
{
    public class BusRepositoryTest
    {
        private readonly BusDTO _busDTO;
        private readonly Bus _busExpected;

        public BusRepositoryTest()
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
            Mock<IInfluxDBSchedule> mock = new();
            mock.SetupSequence(mock => mock.Add(It.IsAny<Bus>()))
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
            Mock<IInfluxDBSchedule> mock = new();
            mock.SetupSequence(mock => mock.GetByBusNumber(_busExpected.BusNumber))
                .ReturnsAsync(_busExpected)
                .ReturnsAsync(null as Bus);

            BusRepository busRepository = new(mock.Object);

            Bus? busActual = busRepository.GetByBusNumber(_busDTO.BusNumber);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);

            busActual = busRepository.GetByBusNumber(_busDTO.BusNumber);
            Assert.Null(busActual);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByDevEUI()
        {
            Mock<IInfluxDBSchedule> mock = new();
            mock.SetupSequence(mock => mock.GetByDevEUI(_busExpected.BusNumber))
                .ReturnsAsync(_busExpected)
                .ReturnsAsync(null as Bus);

            BusRepository busRepository = new(mock.Object);

            Bus? busActual = busRepository.GetBusByDevEUICard(_busDTO.BusNumber);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);

            busActual = busRepository.GetBusByDevEUICard(_busDTO.BusNumber);
            Assert.Null(busActual);
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
            Mock<IInfluxDBSchedule> mock = new();
            mock.SetupSequence(mock => mock.GetAll())
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
