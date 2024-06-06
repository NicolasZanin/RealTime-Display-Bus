using api_csharp_uplink.Entities;

namespace api_csharp_uplink.DB
{
    public interface IInfluxDBBus
    {
        public Task<Bus?> Add(Bus bus);
        public Task<Bus?> GetByBusNumber(int BusNumber);
        public Task<Bus?> GetByDevEUI(int DevEUI);
        public Task<List<Bus>> GetAll();
        public Task Delete(string query);
    }
}
