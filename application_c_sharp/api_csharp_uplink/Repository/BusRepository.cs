using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Repository
{
    public interface IBusRepository
    {
        public Bus? AddBus(Bus bus);
        public Bus? GetByBusNumber(int busNumber);
        public Bus? GetBusByDevEUICard(int busNumber);
        public List<Bus> GetBuses();
    }
    public class BusRepository(IInfluxDBBus InfluxDBBus) : IBusRepository
    {
        private readonly IInfluxDBBus _influxDBBus = InfluxDBBus;

        public Bus? AddBus(Bus bus)
        {
            return _influxDBBus.Add(bus).Result;
        }

        public Bus? GetByBusNumber(int busNumber)
        {
            Bus? bus = _influxDBBus.GetByBusNumber(busNumber).Result;
            return bus;
        }

        public Bus? GetBusByDevEUICard(int devEUI)
        {
            Bus? bus = _influxDBBus.GetByDevEUI(devEUI).Result;
            return bus;
        }

        public List<Bus> GetBuses()
        {
            List<Bus> list = _influxDBBus.GetAll().Result;
            return list;
        }
    }
}
