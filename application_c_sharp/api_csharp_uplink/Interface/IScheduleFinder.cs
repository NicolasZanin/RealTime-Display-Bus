using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IScheduleFinder
{
    public Schedule FindSchedule(string nameStation, int lineNumber, Orientation orientation);
    public List<Schedule> FindScheduleByStationNameOrientation(string nameStation, Orientation orientation);
}