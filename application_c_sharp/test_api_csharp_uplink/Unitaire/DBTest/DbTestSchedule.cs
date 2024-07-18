using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.DBTest;

public class DbTestSchedule : IScheduleRepository
{
    private readonly Dictionary<FindScheduleMap, Schedule> _dico = [];
    
    public Schedule AddSchedule(Schedule schedule)
    {
        FindScheduleMap findScheduleMap =
            new FindScheduleMap(schedule.StationName, schedule.LineNumber, schedule.Orientation);

        if (_dico.TryGetValue(findScheduleMap, out Schedule? scheduleDico))
        {
            scheduleDico.Hours.AddRange(schedule.Hours);
            return schedule;
        }
        
        _dico.Add(findScheduleMap, schedule);
        return schedule;
    }

    public Schedule? FindSchedule(string nameStation, int lineNumber, Orientation orientation)
    {
        _dico.TryGetValue(new FindScheduleMap(nameStation, lineNumber, orientation), out Schedule? schedule);
        return schedule;
    }

    public List<Schedule> FindScheduleByStationNameOrientation(string nameStation, Orientation orientation)
    {
        return _dico.Keys
            .Where(findScheduleMap => findScheduleMap.nameStation.Equals(nameStation) && findScheduleMap.orientation == orientation)
            .Select(map => _dico[map]).ToList();
    }
    
    private record FindScheduleMap(string nameStation, int lineNumber, Orientation orientation);
}