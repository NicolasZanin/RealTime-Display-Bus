using api_csharp_uplink.Dto;
using Moq;
using api_csharp_uplink.Repository;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Entities;
using api_csharp_uplink.DirException;

namespace test_api_csharp_uplink.Unitaire.Composant
{
    public class BusComposantTest
    {
        private readonly BusDTO _busDTO;
        private readonly Bus _busExpected;

        public BusComposantTest()
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

            Bus busActual = busComposant.CreateBus(_busDTO.LineBus, _busDTO.BusNumber, _busDTO.DevEUICard);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);
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

            busComposant.CreateBus(_busDTO.LineBus, _busDTO.BusNumber, _busDTO.DevEUICard);
            Assert.Throws<BusAlreadyCreateException>(() => busComposant.CreateBus(_busDTO.LineBus, _busDTO.BusNumber, _busDTO.DevEUICard));
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

            Bus busActual = busComposant.GetBusByBusNumber(_busExpected.BusNumber);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);

            Assert.Throws<BusNotFoundException>(() => busComposant.GetBusByBusNumber(_busExpected.BusNumber));
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

            Bus busActual = busComposant.GetBusByDevEUICard(_busExpected.BusNumber);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);

            Assert.Throws<BusNotFoundException>(() => busComposant.GetBusByDevEUICard(_busExpected.BusNumber));
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
            List<Bus> buses = busComposant.GetBuses();

            Assert.Empty(buses);

            buses = busComposant.GetBuses();
            Assert.Equal(Assert.Single(buses), busExpected1);

            buses = busComposant.GetBuses();
            Assert.Equal(3, buses.Count);
            Assert.Equal([busExpected1, busExpected2, busExpected3], buses);
        }
    }
}
