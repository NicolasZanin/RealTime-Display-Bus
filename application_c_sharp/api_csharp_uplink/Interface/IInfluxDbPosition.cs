using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IInfluxDbPosition
{
    public Task<PositionBus> Add(PositionBus positionBus);
    public Task<PositionBus?> GetLast(int devEuiNumber);
}