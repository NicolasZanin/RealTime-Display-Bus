using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IPositionRepository
{
    public Task<PositionBus> Add(PositionBus positionBus);
    public Task<PositionBus?> GetLast(string devEuiCard);
}