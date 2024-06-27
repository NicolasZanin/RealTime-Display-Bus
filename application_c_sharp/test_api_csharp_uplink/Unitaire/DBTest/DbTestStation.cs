using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.DBTest;

public class DbTestStation : IStationRepository
{
    
    private readonly List<Station> _stations = [];
    
    public Station Add(Station station)
    {
        _stations.Add(station);
        return station;
    }

    public Station? GetStation(string nameStation)
    {
        Station? station = _stations.Find(station => station.NameStation.Equals(nameStation));
        return station;
    }

    public Station? GetStation(Position position)
    {
        Station? station = _stations.Find(station => station.Position.Equals(position));
        return station;
    }
    
}