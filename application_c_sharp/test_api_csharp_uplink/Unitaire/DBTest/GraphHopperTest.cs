using api_csharp_uplink.Connectors.ExternalEntities;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.DBTest;

public class GraphHopperTest : IGraphHelper
{
    public Task<TimeDistance> GetTimeAndDistance(Position position1, Position position2)
    {
        TimeDistance timeDistance = new TimeDistance(5, 5.0);
        return Task.FromResult(timeDistance);
    }
}