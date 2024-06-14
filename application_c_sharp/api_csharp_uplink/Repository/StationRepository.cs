using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Repository;

public class StationRepository(IInfluxDbStation influxDbStation) : IStationRepository
{
    public Station AddStation(Station station)
    {
        return influxDbStation.Add(station).Result;
    }

    public Station? GetStation(string nameStation)
    {
        return influxDbStation.GetStation(nameStation).Result;
    }

    public Station? GetStation(Position position)
    {
        return influxDbStation.GetStation(position).Result;
    }
}