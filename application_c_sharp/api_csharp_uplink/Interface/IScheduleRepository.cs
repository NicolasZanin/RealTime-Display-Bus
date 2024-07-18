using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IScheduleRepository
{
    Schedule AddSchedule(Schedule schedule);
    Schedule? FindSchedule(string nameStation, int lineNumber, Orientation orientation);
    List<Schedule> FindScheduleByStationNameOrientation(string nameStation, Orientation orientation);
}