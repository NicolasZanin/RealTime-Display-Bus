using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IPositionRepository
{
    public PositionBus AddPosition(PositionBus positionBus);
    public PositionBus? GetLastPosition(string devEuiCard);
}