﻿using api_csharp_uplink.Entities;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Composant;

public class CardComposant(ICardRepository cardRepository) : ICardFinder, ICardRegistration
{
    public Card CreateCard(int lineNumber, string devEuiCard)
    {
        if (cardRepository.GetByDevEui(devEuiCard) != null)
        {
            throw new AlreadyCreateException($"The card with devEuiCard {devEuiCard} already exists");
        }

        if (lineNumber <= 0)
        {
            throw new ValueNotCorrectException("The line number, card number must be greater than 0");
        }

        Card card = new(devEuiCard, lineNumber);

        return cardRepository.Add(card) ?? throw new DbException("Problem with database");
    }

    public Card ModifyCard(int lineNumber, string devEuiCard)
    {
        if (cardRepository.GetByDevEui(devEuiCard) == null)
        {
            throw new NotFoundException($"The card with devEuiCard {devEuiCard} not found");
        }
        
        Card card = new(devEuiCard, lineNumber);
        return cardRepository.Modify(card);
    }

    public Card GetCardByDevEuiCard(string devEuiCard)
    {
        Card? bus = cardRepository.GetByDevEui(devEuiCard);
        return bus ?? throw new NotFoundException($"The card with devEuiCard {devEuiCard} not found");
    }

    public List<Card> GetCards()
    {
        return cardRepository.GetAll();
    }
}