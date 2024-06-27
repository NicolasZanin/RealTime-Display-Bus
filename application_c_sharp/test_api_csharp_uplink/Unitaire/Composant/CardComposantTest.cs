using api_csharp_uplink.Dto;
using Moq;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Entities;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.Composant
{
    public class CardComposantTest
    {
        private readonly CardDto _cardDto = new()
        {
            LineBus = 1,
            DevEuiCard = "0"
        };
        private readonly Card _cardExpected = new("0", 1);

        [Fact]
        [Trait("Category", "Unit")]
        public void TestCreateCard()
        {
            Mock<ICardRepository> mock = new();
            mock.Setup(cardRepository => cardRepository.Add(_cardExpected)).Returns(_cardExpected);
            CardComposant cardComposant = new(mock.Object);

            Card cardActual = cardComposant.CreateCard(_cardDto.LineBus, _cardDto.DevEuiCard);
            Assert.NotNull(cardActual);
            Assert.Equal(_cardExpected, cardActual);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestFalseCreate2CardSameTime()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.Add(_cardExpected))
                .Returns(_cardExpected)
                .Returns(_cardExpected);
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_cardExpected.DevEuiCard))
                .Returns(null as Card)
                .Returns(_cardExpected);

            CardComposant cardComposant = new(mock.Object);

            cardComposant.CreateCard(_cardDto.LineBus, _cardDto.DevEuiCard);
            Assert.Throws<AlreadyCreateException>(() => cardComposant.CreateCard(_cardDto.LineBus, _cardDto.DevEuiCard));
        }
        
        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetCardByDevEui()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_cardExpected.DevEuiCard))
                .Returns(_cardExpected)
                .Returns(null as Card);

            CardComposant cardComposant = new(mock.Object);

            Card cardActual = cardComposant.GetCardByDevEuiCard(_cardExpected.DevEuiCard);
            Assert.NotNull(cardActual);
            Assert.Equal(_cardExpected, cardActual);

            Assert.Throws<NotFoundException>(() => cardComposant.GetCardByDevEuiCard(_cardExpected.DevEuiCard));
        }
        

        [Fact]
        [Trait("Category", "Unit")]
        public void TestModifyCard()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_cardExpected.DevEuiCard))
                .Returns(_cardExpected);
            mock.SetupSequence(cardRepository => cardRepository.Modify(_cardExpected))
                .Returns(_cardExpected);
            CardComposant cardComposant = new(mock.Object);

            Card cardActual = cardComposant.ModifyCard(_cardExpected.LineBus, _cardExpected.DevEuiCard);
            Assert.NotNull(cardActual);
            Assert.Equal(_cardExpected, cardActual);
        }
        
        [Fact]
        [Trait("Category", "Unit")]
        public void TestModifyCardError()
        {
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetByDevEui(_cardExpected.DevEuiCard))
                .Returns(null as Card);
            mock.SetupSequence(cardRepository => cardRepository.Modify(_cardExpected))
                .Returns(_cardExpected);

            CardComposant cardComposant = new(mock.Object);
            Assert.Throws<NotFoundException>(() => cardComposant.ModifyCard(_cardExpected.LineBus, _cardExpected.DevEuiCard));
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void TestGetCards()
        {
            Card cardExpected1 = new("1", 1);

            Card cardExpected2 = new("2", 5);

            Card cardExpected3 = new("3", 5);
            
            Mock<ICardRepository> mock = new();
            mock.SetupSequence(cardRepository => cardRepository.GetAll())
                .Returns([])
                .Returns([cardExpected1])
                .Returns([cardExpected1, cardExpected2, cardExpected3]);

            CardComposant cardComposant = new(mock.Object);
            List<Card> buses = cardComposant.GetCards();

            Assert.Empty(buses);

            buses = cardComposant.GetCards();
            Assert.Equal(Assert.Single(buses), cardExpected1);

            buses = cardComposant.GetCards();
            Assert.Equal(3, buses.Count);
            Assert.Equal([cardExpected1, cardExpected2, cardExpected3], buses);
        }
    }
}
