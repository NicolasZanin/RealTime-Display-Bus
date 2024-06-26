using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.DBTest;

public class DbTestStation : IStationRepository
{
    
    private readonly List<Station> _context = [];
    
    public Station Add(Station station)
    {
        _context.Add(station);
        return station;
    }

    public Station? GetStation(string nameStation)
    {
        Station? station = _context.Find(station => station.NameStation.Equals(nameStation));
        return station;
    }

    public Station? GetStation(Position position)
    {
        Station? station = _context.Find(station => station.Position.Equals(position));
        return station;
    }
    
}