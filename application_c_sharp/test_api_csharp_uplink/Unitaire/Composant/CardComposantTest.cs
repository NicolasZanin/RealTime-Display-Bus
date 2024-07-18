﻿using api_csharp_uplink.Dto;
using api_csharp_uplink.Composant;
using api_csharp_uplink.Entities;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Interface;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Composant;

public class CardComposantTest
{
    private readonly CardDto _cardDto = new()
    {
        LineBus = 1,
        DevEuiCard = "0"
    };
    private readonly Card _cardExpected = new("0", 1);
    private readonly CardComposant _cardComposant;
    
    public CardComposantTest()
    {
        ICardRepository cardRepository = new DbTestCard();
        _cardComposant= new CardComposant(cardRepository);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestCreateCard()
    {
        Card cardActual = _cardComposant.CreateCard(_cardDto.LineBus, _cardDto.DevEuiCard);
        Assert.NotNull(cardActual);
        Assert.Equal(_cardExpected, cardActual);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestFalseCreate2CardSameTime()
    {
        _cardComposant.CreateCard(_cardDto.LineBus, _cardDto.DevEuiCard);
        Assert.Throws<AlreadyCreateException>(() => _cardComposant.CreateCard(_cardDto.LineBus, _cardDto.DevEuiCard));
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetCardByDevEui()
    {
        Assert.Throws<NotFoundException>(() => _cardComposant.GetCardByDevEuiCard(_cardExpected.DevEuiCard));
        
        _cardComposant.CreateCard(_cardDto.LineBus, _cardDto.DevEuiCard);
        Card cardActual = _cardComposant.GetCardByDevEuiCard(_cardExpected.DevEuiCard);
        Assert.NotNull(cardActual);
        Assert.Equal(_cardExpected, cardActual);
    }
    

    [Fact]
    [Trait("Category", "Unit")]
    public void TestModifyCard()
    {
        _cardComposant.CreateCard(_cardDto.LineBus, _cardDto.DevEuiCard);
        Card cardActual = _cardComposant.ModifyCard(_cardExpected.LineBus, _cardExpected.DevEuiCard);
        Assert.NotNull(cardActual);
        Assert.Equal(_cardExpected, cardActual);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestModifyCardError()
    {
        Assert.Throws<NotFoundException>(() => _cardComposant.ModifyCard(_cardExpected.LineBus, _cardExpected.DevEuiCard));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestGetCards()
    {
        Card cardExpected1 = new("1", 1);

        Card cardExpected2 = new("2", 5);

        Card cardExpected3 = new("3", 5);
        
        List<Card> cards = _cardComposant.GetCards();
        Assert.Empty(cards);
        
        _cardComposant.CreateCard(cardExpected1.LineBus, cardExpected1.DevEuiCard);
        cards = _cardComposant.GetCards();
        Assert.Equal(Assert.Single(cards), cardExpected1);
        
        _cardComposant.CreateCard(cardExpected2.LineBus, cardExpected2.DevEuiCard);
        _cardComposant.CreateCard(cardExpected3.LineBus, cardExpected3.DevEuiCard);
        cards = _cardComposant.GetCards();
        Assert.Equal(3, cards.Count);
        Assert.Equal([cardExpected1, cardExpected2, cardExpected3], cards);
    }
}