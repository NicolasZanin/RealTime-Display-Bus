using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IStationFinder
{
    public Station GetStation(string nameStation);
    public Station GetStation(double latitude, double longitude);
}