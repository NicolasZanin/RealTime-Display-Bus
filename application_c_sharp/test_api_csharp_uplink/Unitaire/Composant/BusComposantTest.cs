using api_csharp_uplink.Dto;
using Moq;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Entities;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.Composant
{
    public class BusComposantTest
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
            mock.Setup(cardRepository => cardRepository.Add(_busExpected)).Returns(_busExpected);
            BusComposant busComposant = new(mock.Object);

            Bus busActual = busComposant.CreateBus(_busDto.LineBus, _busDto.BusNumber, _busDto.DevEuiCard);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestFalseCreate2BusSameTime()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.Add(_busExpected))
                .Returns(_busExpected)
                .Returns(_busExpected);
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_busExpected.DevEuiCard))
                .Returns(null as Bus)
                .Returns(_busExpected);

            BusComposant busComposant = new(mock.Object);

            busComposant.CreateBus(_busDto.LineBus, _busDto.BusNumber, _busDto.DevEuiCard);
            Assert.Throws<BusAlreadyCreateException>(() => busComposant.CreateBus(_busDto.LineBus, _busDto.BusNumber, _busDto.DevEuiCard));
        }
        

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBusByDevEui()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_busExpected.DevEuiCard))
                .Returns(_busExpected)
                .Returns(null as Bus);

            BusComposant busComposant = new(mock.Object);

            Bus busActual = busComposant.GetBusByDevEuiCard(_busExpected.DevEuiCard);
            Assert.NotNull(busActual);
            Assert.Equal(_busExpected, busActual);

            Assert.Throws<BusDevEuiCardNotFoundException>(() => busComposant.GetBusByDevEuiCard(_busExpected.DevEuiCard));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetBuses()
        {
            Bus busExpected1 = new(1, "1", 1);

            Bus busExpected2 = new(2, "2", 5);

            Bus busExpected3 = new(3, "3", 5);
            
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetAll())
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
