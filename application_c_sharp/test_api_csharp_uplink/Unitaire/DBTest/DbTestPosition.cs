namespace test_api_csharp_uplink.Unitaire.DBTest;

using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

public class DbTestPosition : IInfluxDbPosition
{
    private readonly List<PositionBus> _context = [];

    public Task<PositionBus> Add(PositionBus positionBus)
    {
        _context.Add(positionBus);
        return Task.FromResult(positionBus);
    }

    public Task<PositionBus?> GetLast(int devEuiNumber)
    {
        List<PositionBus> list = _context.FindAll(position => position.DevEuiNumber == devEuiNumber);
        return Task.FromResult(list.Count > 0 ? list[^1] : null);
    }
}