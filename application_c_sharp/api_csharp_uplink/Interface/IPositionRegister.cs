using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IPositionRegister
{
    public PositionCard AddPosition(double latitude, double longitude, string devEuiCard);
    public PositionCard GetLastPosition(string devEuiCard);
}