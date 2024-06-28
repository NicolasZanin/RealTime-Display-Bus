using api_csharp_uplink.Entities;
using api_csharp_uplink.Repository;
using api_csharp_uplink.DirException;

namespace api_csharp_uplink.Composant
{
    public interface IScheduleService
    {
        Schedule StationSchedulesAller(string station);
        Schedule StationSchedulesRetour(string station);
        List<Schedule> SchedulesAller();
        List<Schedule> SchedulesRetour();
        Bus CreateBus(int lineNumber, int busNumber, int devEUICard);
        List<Bus> GetBuses();
        Bus GetBusByBusNumber(int busNumber);
        Bus GetBusByDevEUICard(int busNumber);
    }

    public class ScheduleComposant(IScheduleRepository scheduleRepository) : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository = scheduleRepository;

        public Schedule StationSchedulesAller(string station)
        {
            Schedule? schedule = _scheduleRepository.GetAllerByStationName(station);
            return schedule ?? throw new ScheduleNotFoundException(station);

          
        }

        public Schedule StationSchedulesRetour(string station)
        {
            Schedule? schedule = _scheduleRepository.GetRetourByStationName(station);
            return schedule ?? throw new ScheduleNotFoundException(station);
        }

        public List<Schedule> SchedulesAller()
        {
            List<Schedule>? schedules = _scheduleRepository.GetSchedulesAller();
            return schedules ?? throw new Exception();
        }

        public List<Schedule> SchedulesRetour()
        {
            List<Schedule>? schedules = _scheduleRepository.GetSchedulesRetour();
            return schedules ?? throw new Exception();
        }

        public Bus GetBusByBusNumber(int busNumber)
        {
            //Bus? bus = busRepository.GetByBusNumber(busNumber);
            //return bus ?? throw new BusNotFoundException(busNumber);
            return null;
        }

        public Bus GetBusByDevEUICard(int busNumber)
        {
            // Bus? bus = busRepository.GetBusByDevEUICard(busNumber);
            //return bus ?? throw new BusNotFoundException(busNumber);
            return null;
        }

        public List<Bus> GetBuses()
        {
            // return busRepository.GetBuses();
            return null;
        }

        public Bus CreateBus(int lineNumber, int busNumber, int devEUICard)
        {
            throw new NotImplementedException();
        }
    }
}
