using api_csharp_uplink.Entities;
using api_csharp_uplink.Repository;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Repository;

public class StationRepositoryTest
{
    private readonly DbTestStation _dbStation = new();
    private readonly Station _stationStation1 = new( new Position(15.0, 14.0), "Station1");
    
    [Fact]
    [Trait("Category", "Unit")]
    public void AddStationTest()
    {
        StationRepository stationRepository = new(_dbStation);
        
        Station stationActual = stationRepository.AddStation(_stationStation1);
        Assert.NotNull(stationActual);
        Assert.Equal(_stationStation1, stationActual);
        
        Station? stationActual2 = stationRepository.GetStation("Station1");
        Assert.NotNull(stationActual2);
        Assert.Equal(_stationStation1, stationActual2);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByNameTest()
    {
        StationRepository stationRepository = new(_dbStation);
        
        stationRepository.AddStation(_stationStation1);
        Station? stationActual = stationRepository.GetStation("Station1");
        Assert.NotNull(stationActual);
        Assert.Equal(_stationStation1, stationActual);
        
        stationActual = stationRepository.GetStation("Station2");
        Assert.Null(stationActual);
        
        Station stationStation2 = new Station(new Position(15.0, 14.0), "Station2");
        stationRepository.AddStation(stationStation2);
        
        stationActual = stationRepository.GetStation("Station2");
        Assert.NotNull(stationActual);
        Assert.Equal(stationStation2, stationActual);
        
        stationActual = stationRepository.GetStation("Station1");
        Assert.NotNull(stationActual);
        Assert.Equal(_stationStation1, stationActual);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByPositionTest()
    {
        StationRepository stationRepository = new(_dbStation);
        Position positionGood = new Position(15.0, 14.0);
        
        stationRepository.AddStation(_stationStation1);
        Station? stationActual = stationRepository.GetStation(positionGood);
        Assert.NotNull(stationActual);
        Assert.Equal(_stationStation1, stationActual);
        
        Position positionBad = new Position(15.0, 14.1);
        stationActual = stationRepository.GetStation(positionBad);
        Assert.Null(stationActual);
        
        positionBad = new Position(15.1, 14.0);
        stationActual = stationRepository.GetStation(positionBad);
        Assert.Null(stationActual);
        
        Station stationStation2 = new Station(positionBad, "Station2");
        stationRepository.AddStation(stationStation2);
        
        stationActual = stationRepository.GetStation(positionBad);
        Assert.NotNull(stationActual);
        Assert.Equal(stationStation2, stationActual);
        
        stationActual = stationRepository.GetStation(positionGood);
        Assert.NotNull(stationActual);
        Assert.Equal(_stationStation1, stationActual);
    }
}