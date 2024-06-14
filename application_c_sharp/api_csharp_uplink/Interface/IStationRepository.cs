using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IStationRepository
{
    public Station AddStation(Station station);
    public Station? GetStation(string nameStation);
    public Station? GetStation(Position position);
}