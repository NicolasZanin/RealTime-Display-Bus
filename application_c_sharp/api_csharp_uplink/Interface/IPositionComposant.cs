using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IPositionComposant
{
    public PositionBus AddPosition(double latitude, double longitude, string devEuiCard);
    public PositionBus GetLastPosition(string devEuiCard);
}