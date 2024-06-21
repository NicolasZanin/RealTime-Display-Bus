using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface ICardFinder
{
    List<Bus> GetBuses();
    Bus GetBusByDevEuiCard(string devEuiCard);
}