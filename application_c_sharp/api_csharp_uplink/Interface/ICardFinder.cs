using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface ICardFinder
{
    List<Card> GetCards();
    Card GetCardByDevEuiCard(string devEuiCard);
}