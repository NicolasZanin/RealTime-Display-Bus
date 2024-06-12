using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Repository;

public class PositionRepository(IInfluxDbPosition influxDbPosition) : IPositionRepository
{
    public PositionBus AddPosition(PositionBus positionBus)
    {
        return influxDbPosition.Add(positionBus).Result;
    }
    
    public PositionBus? GetLastPosition(string devEuiCard)
    {
        return influxDbPosition.GetLast(devEuiCard).Result;
    }
}