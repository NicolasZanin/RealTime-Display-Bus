using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Repository
{
    public interface IScheduleRepository
    {
        public Schedule? AddSchedule(Schedule schedule);
        public Schedule? GetAllerByStationName(string station);
        public Schedule? GetRetourByStationName(string station);
        public List<Schedule> GetSchedulesAller();
        public List<Schedule> GetSchedulesRetour();
    }
    public class ScheduleRepository(IInfluxDBSchedule influxDBSchedule) : IScheduleRepository
    {
        private readonly IInfluxDBSchedule _influxDBSchedule = influxDBSchedule;

        public Schedule? AddSchedule(Schedule schedule)
        {
            return _influxDBSchedule.Add(schedule).Result;
        }

        public Schedule? GetAllerByStationName(string station)
        {
            return _influxDBSchedule.GetScheduleAllerByStationName(station).Result;
        }
        public Schedule? GetRetourByStationName(string station)
        {
            return _influxDBSchedule.GetScheduleRetourByStationName(station).Result;
        }
        public List<Schedule> GetSchedulesAller()
        {
            return _influxDBSchedule.GetAllSchedulesAller().Result;
        }
        public List<Schedule> GetSchedulesRetour()
        {
            return _influxDBSchedule.GetAllSchedulesRetour().Result;
        }
    }
}
