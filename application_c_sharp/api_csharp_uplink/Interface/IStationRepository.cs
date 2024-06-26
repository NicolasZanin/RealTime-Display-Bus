using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IStationRepository
{
    public Station Add(Station station);
    public Station? GetStation(string nameStation);
    public Station? GetStation(Position position);
}