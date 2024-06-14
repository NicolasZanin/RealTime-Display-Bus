using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IStationComposant
{
    public Station AddStation(double latitude, double longitude, string nameStation);
    public Station GetStation(string nameStation);
    public Station GetStation(double latitude, double longitude);
}