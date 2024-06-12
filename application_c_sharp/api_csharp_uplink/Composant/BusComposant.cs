using api_csharp_uplink.Entities;
using api_csharp_uplink.Repository;
using api_csharp_uplink.DirException;

namespace api_csharp_uplink.Composant
{
    public interface IBusService
    {
        Bus CreateBus(int lineNumber, int busNumber, string devEuiCard);
        List<Bus> GetBuses();
        Bus GetBusByBusNumber(int busNumber);
        Bus GetBusByDevEuiCard(string devEuiCard);
    }

    public class BusComposant(IBusRepository busRepository) : IBusService
    {
        public Bus CreateBus(int lineNumber, int busNumber, string devEuiCard)
        {
            if (busRepository.GetByBusNumber(busNumber) != null)
            {
                throw new BusAlreadyCreateException(busNumber);
            }

            if (lineNumber <= 0 || busNumber < 0)
            {
                throw new ValueNotCorrectException("The line number, bus number must be greater than 0");
            }

            Bus bus = new(busNumber, devEuiCard, lineNumber);

            return busRepository.AddBus(bus)?? throw new DbException("Problem with database");
        }

        public Bus GetBusByBusNumber(int busNumber)
        {
            Bus? bus = busRepository.GetByBusNumber(busNumber);
            return bus ?? throw new BusNotFoundException(busNumber);
        }

        public Bus GetBusByDevEuiCard(string devEuiCard)
        {
            Bus? bus = busRepository.GetBusByDevEuiCard(devEuiCard);
            return bus ?? throw new BusDevEuiCardNotFoundException(devEuiCard);
        }

        public List<Bus> GetBuses()
        {
            return busRepository.GetBuses();
        }
    }
}
