using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Repository
{
    public interface IBusRepository
    {
        public Bus? AddBus(Bus bus);
        public Bus? GetByBusNumber(int busNumber);
        public Bus? GetBusByDevEuiCard(string devEuiCard);
        public List<Bus> GetBuses();
    }
    public class BusRepository(IInfluxDbBus influxDbBus) : IBusRepository
    {
        public Bus? AddBus(Bus bus)
        {
            return influxDbBus.Add(bus).Result;
        }

        public Bus? GetByBusNumber(int busNumber)
        {
            Bus? bus = influxDbBus.GetByBusNumber(busNumber).Result;
            return bus;
        }

        public Bus? GetBusByDevEuiCard(string devEuiCard)
        {
            Bus? bus = influxDbBus.GetByDevEui(devEuiCard).Result;
            return bus;
        }

        public List<Bus> GetBuses()
        {
            List<Bus> list = influxDbBus.GetAll().Result;
            return list;
        }
    }
}
