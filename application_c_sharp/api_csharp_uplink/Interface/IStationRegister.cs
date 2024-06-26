using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IStationRegister
{
    public Station AddStation(double latitude, double longitude, string nameStation);
}