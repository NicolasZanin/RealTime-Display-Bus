using api_csharp_uplink.Composant;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Repository;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Composant;

public class StationComposantTest
{
    private readonly StationComposant _stationComposant;
    private readonly Station _stationStation1 = new(15.0, 14.0, "Station1");
    
    public StationComposantTest()
    {
        StationRepository stationRepository = new(new DbTestStation());
        _stationComposant = new(stationRepository);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void AddStationTest()
    {
        Station stationActual = _stationComposant.AddStation(_stationStation1.Position.Latitude, 
            _stationStation1.Position.Longitude, _stationStation1.NameStation);
        Assert.NotNull(stationActual);
        Assert.Equal(_stationStation1, stationActual);
        
        Station stationActual2 = _stationComposant.GetStation("Station1");
        Assert.NotNull(stationActual2);
        Assert.Equal(_stationStation1, stationActual2);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void AddErrorStationTest()
    {
        _stationComposant.AddStation(_stationStation1.Position.Latitude,
            _stationStation1.Position.Longitude, _stationStation1.NameStation);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _stationComposant.AddStation(90.01,
            _stationStation1.Position.Longitude, "Test"));
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _stationComposant.AddStation(-90.01,
            _stationStation1.Position.Longitude, "Test"));
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _stationComposant.AddStation(_stationStation1.Position.Latitude,
            180.01, "Test"));
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _stationComposant.AddStation(_stationStation1.Position.Latitude,
            -180.01, "Test"));
        
        Assert.Throws<ArgumentNullException>(() => _stationComposant.AddStation(_stationStation1.Position.Latitude,
            _stationStation1.Position.Longitude, ""));
        
        Assert.Throws<AlreadyCreateException>(() => _stationComposant.AddStation(_stationStation1.Position.Latitude,
            _stationStation1.Position.Longitude, _stationStation1.NameStation));
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByNameTest()
    {
        Station stationStation2 = new Station(15.5, 14.0, "Station2");
        
        _stationComposant.AddStation(_stationStation1.Position.Latitude, 
            _stationStation1.Position.Longitude, _stationStation1.NameStation);
        
        Station stationActual = _stationComposant.GetStation("Station1");
        Assert.NotNull(stationActual);
        Assert.Equal(_stationStation1, stationActual);
        
        Assert.Throws<NotFoundException>(() => _stationComposant.GetStation("Station2"));
        
        _stationComposant.AddStation(stationStation2.Position.Latitude, 
            stationStation2.Position.Longitude, stationStation2.NameStation);
        
        stationActual = _stationComposant.GetStation("Station2");
        Assert.NotNull(stationActual);
        Assert.Equal(stationStation2, stationActual);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByNameErrorTest()
    {
        Assert.Throws<ArgumentNullException>(() => _stationComposant.GetStation(""));
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByPositionTest()
    {
        Position positionStation2 = new(15.5, 14.0);
        Station stationStation2 = new Station(positionStation2, "Station2");
        
        _stationComposant.AddStation(_stationStation1.Position.Latitude, 
            _stationStation1.Position.Longitude, _stationStation1.NameStation);
        
        Station stationActual = _stationComposant.GetStation(_stationStation1.Position.Latitude, 
            _stationStation1.Position.Longitude);
        Assert.NotNull(stationActual);
        Assert.Equal(_stationStation1, stationActual);
        
        Assert.Throws<NotFoundException>(() => _stationComposant.GetStation(positionStation2.Latitude, positionStation2.Longitude));
        
        _stationComposant.AddStation(stationStation2.Position.Latitude, 
            stationStation2.Position.Longitude, stationStation2.NameStation);
        
        stationActual = _stationComposant.GetStation(positionStation2.Latitude, positionStation2.Longitude);
        Assert.NotNull(stationActual);
        Assert.Equal(stationStation2, stationActual);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByPositionErrorTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _stationComposant.GetStation(90.1, 14.5));
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _stationComposant.GetStation(-90.1, 14.5));
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _stationComposant.GetStation(14.5, 180.1));
        
        Assert.Throws<ArgumentOutOfRangeException>(() => _stationComposant.GetStation(14.5, -180.1));
    }
}