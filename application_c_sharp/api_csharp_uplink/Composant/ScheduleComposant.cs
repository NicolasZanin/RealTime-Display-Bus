using api_csharp_uplink.Entities;
using api_csharp_uplink.Repository;
using api_csharp_uplink.DirException;

namespace api_csharp_uplink.Composant
{
    public interface IScheduleService
    {
        Schedule StationSchedules(String station);
        Bus CreateBus(int lineNumber, int busNumber, int devEUICard);
        List<Bus> GetBuses();
        Bus GetBusByBusNumber(int busNumber);
        Bus GetBusByDevEUICard(int busNumber);
    }

    public class ScheduleComposant(IBusRepository busRepository) : IScheduleService
    {
        private readonly IBusRepository busRepository = busRepository;

        public Schedule StationSchedules(string station)
        {
            return null;
        }

        public Bus GetBusByBusNumber(int busNumber)
        {
            Bus? bus = busRepository.GetByBusNumber(busNumber);
            return bus ?? throw new BusNotFoundException(busNumber);
        }

        public Bus GetBusByDevEUICard(int busNumber)
        {
            Bus? bus = busRepository.GetBusByDevEUICard(busNumber);
            return bus ?? throw new BusNotFoundException(busNumber);
        }

        public List<Bus> GetBuses()
        {
            return busRepository.GetBuses();
        }

        public Bus CreateBus(int lineNumber, int busNumber, int devEUICard)
        {
            throw new NotImplementedException();
        }
    }
}
