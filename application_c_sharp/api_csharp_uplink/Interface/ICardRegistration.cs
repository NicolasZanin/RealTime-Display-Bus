using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface ICardRegistration
{
    Card CreateCard(int lineNumber, string devEuiCard);
}