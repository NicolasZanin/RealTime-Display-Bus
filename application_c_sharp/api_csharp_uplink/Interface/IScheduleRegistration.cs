using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IScheduleRegistration
{
    Schedule AddSchedule(string nameStation, int lineNumber, string orientation, List<DateTime> hours);
}