using api_csharp_uplink.Composant;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Composant;

public class PositionComposantTest
{
    private readonly PositionComposant _positionComposant;
    private readonly PositionCard _positionBus15140 = new(new Position(15.0, 14.0), "0");
    
    public PositionComposantTest()
    {
        IPositionRepository positionRepository = new DbTestPosition();
        _positionComposant = new PositionComposant(positionRepository);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void AddPositionTest()
    {
        PositionCard positionCardActual = _positionComposant.AddPosition(_positionBus15140.Position.Latitude, 
            _positionBus15140.Position.Longitude, _positionBus15140.DevEuiCard);
        Assert.NotNull(positionCardActual);
        Assert.Equal(_positionBus15140, positionCardActual);
        
        PositionCard positionCardActual2 = _positionComposant.GetLastPosition("0");
        Assert.NotNull(positionCardActual2);
        Assert.Equal(_positionBus15140, positionCardActual2);
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
        PositionCard positionBus15141 = new PositionCard(new Position(15.0, 14.0), "1");
        PositionCard positionBus14140 = new PositionCard(new Position(14.5, 14.0), "0");
        
        _positionComposant.AddPosition(_positionBus15140.Position.Latitude, 
            _positionBus15140.Position.Longitude, _positionBus15140.DevEuiCard);
        _positionComposant.AddPosition(positionBus15141.Position.Latitude, 
            positionBus15141.Position.Longitude, positionBus15141.DevEuiCard);
        
        PositionCard positionActual = _positionComposant.GetLastPosition("0");
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