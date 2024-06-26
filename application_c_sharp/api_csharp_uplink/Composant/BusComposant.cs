using api_csharp_uplink.Entities;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Composant
{
    public class BusComposant(ICardRepository cardRepository) : ICardFinder, ICardRegistration
    {
        public Bus CreateBus(int lineNumber, int busNumber, string devEuiCard)
        {
            if (cardRepository.GetByDevEui(devEuiCard) != null)
            {
                throw new BusAlreadyCreateException(busNumber);
            }

            if (lineNumber <= 0 || busNumber < 0)
            {
                throw new ValueNotCorrectException("The line number, bus number must be greater than 0");
            }

            Bus bus = new(busNumber, devEuiCard, lineNumber);

            return cardRepository.Add(bus) ?? throw new DbException("Problem with database");
        }

        public Bus GetBusByDevEuiCard(string devEuiCard)
        {
            Bus? bus = cardRepository.GetByDevEui(devEuiCard);
            return bus ?? throw new BusDevEuiCardNotFoundException(devEuiCard);
        }

        public List<Bus> GetBuses()
        {
            return cardRepository.GetAll();
        }
    }
}
