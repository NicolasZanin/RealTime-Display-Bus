using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface ICardRepository
{
    public Bus Add(Bus bus);
    public Bus? GetByDevEui(string devEuiCard);
    public List<Bus> GetAll();
}