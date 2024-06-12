using api_csharp_uplink.Composant;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Repository;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Composant;

public class PositionComposantTest
{
    private readonly PositionComposant _positionComposant;
    private readonly PositionBus _positionBus15140 = new(new Position(15.0, 14.0), "0");
    
    public PositionComposantTest()
    {
        PositionRepository positionRepository = new(new DbTestPosition());
        _positionComposant = new(positionRepository);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void AddPositionTest()
    {
        PositionBus positionBusActual = _positionComposant.AddPosition(_positionBus15140.Position.Latitude, 
            _positionBus15140.Position.Longitude, _positionBus15140.DevEuiCard);
        Assert.NotNull(positionBusActual);
        Assert.Equal(_positionBus15140, positionBusActual);
        
        PositionBus positionBusActual2 = _positionComposant.GetLastPosition("0");
        Assert.NotNull(positionBusActual2);
        Assert.Equal(_positionBus15140, positionBusActual2);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void AddErrorPositionTest()
    {
        Assert.Throws<ValueNotCorrectException>(() => _positionComposant.AddPosition(90.01,
            _positionBus15140.Position.Longitude, _positionBus15140.DevEuiCard));
        
        Assert.Throws<ValueNotCorrectException>(() => _positionComposant.AddPosition(-90.01,
            _positionBus15140.Position.Longitude, _positionBus15140.DevEuiCard));
        
        Assert.Throws<ValueNotCorrectException>(() => _positionComposant.AddPosition(_positionBus15140.Position.Latitude,
            180.01, _positionBus15140.DevEuiCard));
        
        Assert.Throws<ValueNotCorrectException>(() => _positionComposant.AddPosition(_positionBus15140.Position.Latitude,
            -180.01, _positionBus15140.DevEuiCard));
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetPositionTest()
    {
        PositionBus positionBus15141 = new PositionBus(new Position(15.0, 14.0), "1");
        PositionBus positionBus14140 = new PositionBus(new Position(14.5, 14.0), "0");
        
        _positionComposant.AddPosition(_positionBus15140.Position.Latitude, 
            _positionBus15140.Position.Longitude, _positionBus15140.DevEuiCard);
        _positionComposant.AddPosition(positionBus15141.Position.Latitude, 
            positionBus15141.Position.Longitude, positionBus15141.DevEuiCard);
        
        PositionBus positionActual = _positionComposant.GetLastPosition("0");
        Assert.NotNull(positionActual);
        Assert.Equal(_positionBus15140, positionActual);
        
        positionActual = _positionComposant.GetLastPosition("1");
        Assert.NotNull(positionActual);
        Assert.Equal(positionBus15141, positionActual);

        _positionComposant.AddPosition(positionBus14140.Position.Latitude,
            positionBus14140.Position.Longitude, positionBus14140.DevEuiCard);
        
        positionActual = _positionComposant.GetLastPosition("0");
        Assert.NotNull(positionActual);
        Assert.Equal(positionBus14140, positionActual);
        
        positionActual = _positionComposant.GetLastPosition("1");
        Assert.NotNull(positionActual);
        Assert.Equal(positionBus15141, positionActual);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetErrorPositionTest()
    {
        Assert.Throws<PositionDevEuiNumberException>(() => _positionComposant.GetLastPosition("-1"));
    }
}