using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IGraphPosition
{
    public Task<int> RegisterPositionCard(Card card, Position position);
}