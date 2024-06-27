using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IPositionRepository
{
    public PositionCard Add(PositionCard positionCard);
    public PositionCard? GetLast(string devEuiCard);
}