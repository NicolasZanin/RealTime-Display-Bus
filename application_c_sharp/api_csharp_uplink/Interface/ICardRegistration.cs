using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface ICardRegistration
{
    Bus CreateBus(int lineNumber, int busNumber, string devEuiCard);
}