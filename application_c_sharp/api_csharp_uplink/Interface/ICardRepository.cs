using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface ICardRepository
{
    public Card Add(Card card);
    public Card? GetByDevEui(string devEuiCard);
    public List<Card> GetAll();
}