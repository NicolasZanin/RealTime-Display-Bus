using api_csharp_uplink.Entities;

namespace api_csharp_uplink.DB
{
    public interface IInfluxDBSchedule
    {
        public Task<Schedule?> Add(Schedule schedule);
        public Task<Schedule?> GetScheduleAllerByStationName(string station);
        public Task<Schedule?> GetScheduleRetourByStationName(string station);
        public Task<Tuple<string, double, double>?> GetPositionByStationName(string station);
        public Task<List<Tuple<string,double,double>>> GetAllPosition();
        public Task<List<Schedule>> GetAllSchedulesAller();
        public Task<List<Schedule>> GetAllSchedulesRetour();
        public Task Delete(string query);
    }
}
