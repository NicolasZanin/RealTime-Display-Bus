using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface ICardRepository
{
    public Task<Bus?> Add(Bus bus);
    public Task<Bus?> GetByDevEui(string devEuiCard);
    public Task<List<Bus>> GetAll();
}