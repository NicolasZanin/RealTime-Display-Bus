using api_csharp_uplink.Entities;
using api_csharp_uplink.Repository;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Repository;

public class PositionRepositoryTest
{
    private readonly DbTestPosition _dbTestPosition = new();
    private readonly PositionBus _positionBus15140 = new(new Position(15.0, 14.0), "0");
    
    [Fact]
    [Trait("Category", "Unit")]
    public void AddPositionTest()
    {
        PositionRepository positionRepository = new(_dbTestPosition);
        
        PositionBus positionBusActual = positionRepository.AddPosition(_positionBus15140);
        Assert.NotNull(positionBusActual);
        Assert.Equal(_positionBus15140, positionBusActual);
        
        PositionBus? positionBusActual2 = positionRepository.GetLastPosition("0");
        Assert.NotNull(positionBusActual2);
        Assert.Equal(_positionBus15140, positionBusActual2);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetPositionTest()
    {
        PositionBus positionBus15141 = new PositionBus(new Position(15.0, 14.0), "1");
        PositionBus positionBus14140 = new PositionBus(new Position(14.5, 14.0), "0");
        PositionRepository positionRepository = new(_dbTestPosition);
        
        positionRepository.AddPosition(_positionBus15140);
        positionRepository.AddPosition(positionBus15141);
        
        PositionBus? positionActual = positionRepository.GetLastPosition("0");
        Assert.NotNull(positionActual);
        Assert.Equal(_positionBus15140, positionActual);
        
        positionActual = positionRepository.GetLastPosition("1");
        Assert.NotNull(positionActual);
        Assert.Equal(positionBus15141, positionActual);
        
        positionRepository.AddPosition(positionBus14140);
        
        positionActual = positionRepository.GetLastPosition("0");
        Assert.NotNull(positionActual);
        Assert.Equal(positionBus14140, positionActual);
        
        positionActual = positionRepository.GetLastPosition("1");
        Assert.NotNull(positionActual);
        Assert.Equal(positionBus15141, positionActual);
        
        positionActual = positionRepository.GetLastPosition("2");
        Assert.Null(positionActual);
    }
}