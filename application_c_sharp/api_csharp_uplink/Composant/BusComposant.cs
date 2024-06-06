using api_csharp_uplink.Entities;
using api_csharp_uplink.Repository;
using api_csharp_uplink.DirException;

namespace api_csharp_uplink.Composant
{
    public interface IBusService
    {
        Bus CreateBus(int lineNumber, int busNumber, int devEUICard);
        List<Bus> GetBuses();
        Bus GetBusByBusNumber(int busNumber);
        Bus GetBusByDevEUICard(int busNumber);
    }

    public class BusComposant(IBusRepository busRepository) : IBusService
    {
        private readonly IBusRepository busRepository = busRepository;

        public Bus CreateBus(int lineNumber, int busNumber, int devEUICard)
        {
            if (busRepository.GetByBusNumber(busNumber) != null)
            {
                throw new BusAlreadyCreateException(busNumber);
            }

            if (lineNumber <= 0 || busNumber < 0 || devEUICard < 0)
            {
                throw new ValueNotCorrectException("The line number, bus number and devEUI card must be greater than 0");
            }

            Bus bus = new()
            {
                LineBus = lineNumber,
                BusNumber = busNumber,
                DevEUICard = devEUICard
            };

            return busRepository.AddBus(bus)?? throw new Exception("Problem with database");
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
    }
}
