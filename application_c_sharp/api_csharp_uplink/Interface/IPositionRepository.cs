using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IPositionRepository
{
    public PositionBus Add(PositionBus positionBus);
    public PositionBus? GetLast(string devEuiCard);
}