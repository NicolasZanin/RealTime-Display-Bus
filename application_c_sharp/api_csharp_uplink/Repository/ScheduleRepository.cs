using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Repository
{
    public interface IScheduleRepository
    {
        public Schedule? AddSchedule(Schedule schedule);
        public Schedule? GetByStationName(string station);
        public List<Schedule> GetSchedules();
    }
    public class ScheduleRepository(IInfluxDBSchedule influxDBSchedule) : IScheduleRepository
    {
        private readonly IInfluxDBSchedule _influxDBSchedule = influxDBSchedule;

        public Schedule? AddSchedule(Schedule schedule)
        {
            return _influxDBSchedule.Add(schedule).Result;
        }

        public Schedule? GetByStationName(string station)
        {
            return _influxDBSchedule.GetScheduleByStationName(station).Result;
        }

        public List<Schedule> GetSchedules()
        {
            throw new NotImplementedException();
        }
    }
}
