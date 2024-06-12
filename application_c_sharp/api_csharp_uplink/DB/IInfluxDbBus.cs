using api_csharp_uplink.Entities;

namespace api_csharp_uplink.DB
{
    public interface IInfluxDbBus
    {
        public Task<Bus?> Add(Bus bus);
        public Task<Bus?> GetByBusNumber(int busNumber);
        public Task<Bus?> GetByDevEui(string devEuiCard);
        public Task<List<Bus>> GetAll();
    }
}
