using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.DBTest;

public class DbTestCard : ICardRepository
{
    private List<Card> _cards = [];
    
    public Card Add(Card card)
    {
        _cards.Add(card);
        return card;
    }

    public Card? GetByDevEui(string devEuiCard)
    {
        return _cards.Find(card => card.DevEuiCard == devEuiCard);
    }

    public Card Modify(Card card)
    {
        _cards = _cards.Where(cardd => cardd.DevEuiCard != card.DevEuiCard).ToList();
        _cards.Add(card);
        return card;
    }

    public List<Card> GetAll()
    {
        return _cards;
    }
}