using api_csharp_uplink.Composant;
using api_csharp_uplink.Controllers;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Controllers;

public class StationControllerTest
{
    private readonly StationController _stationController;
    private readonly StationDto _stationDtoStation1 = new(){ NameStation = "Station1", 
        Position = new PositionDto { Latitude = 15.0, Longitude = 14.0 } };
    
    public StationControllerTest()
    {
        StationRepository stationRepository = new(new DbTestStation());
        StationComposant stationComposant = new(stationRepository);
        _stationController = new StationController(stationComposant);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void AddStationTest()
    {
        IActionResult actionResult = _stationController.AddStation(_stationDtoStation1);
        actionResult.Should().BeOfType<CreatedResult>();
        CreatedResult createdResult = (CreatedResult) actionResult;
        createdResult.Should().NotBeNull();
        createdResult.Value.Should().BeEquivalentTo(_stationDtoStation1);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void AddStationErrorTest()
    {
        StationDto stationDtoErrorLatitude = new() { NameStation = _stationDtoStation1.NameStation, 
            Position = new PositionDto { Latitude = 91.0, Longitude = _stationDtoStation1.Position.Longitude  } };
        
        IActionResult actionResult = _stationController.AddStation(stationDtoErrorLatitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        
        stationDtoErrorLatitude.Position.Latitude = -91.0;
        actionResult = _stationController.AddStation(stationDtoErrorLatitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        
        StationDto stationDtoErrorLongitude = new() { NameStation = _stationDtoStation1.NameStation, 
            Position = new PositionDto { Latitude = _stationDtoStation1.Position.Latitude, Longitude = 180.01  } };
        
        actionResult = _stationController.AddStation(stationDtoErrorLatitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        
        stationDtoErrorLongitude.Position.Longitude = -180.01;
        actionResult = _stationController.AddStation(stationDtoErrorLatitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByNameTest()
    {
        StationDto stationDtoStation2 = new(){ NameStation = "Station2", 
            Position = new PositionDto { Latitude = 15.0, Longitude = 14.0 } };
        
        _stationController.AddStation(_stationDtoStation1);
        
        IActionResult actionResult = _stationController.GetStationByName(_stationDtoStation1.NameStation);
        actionResult.Should().BeOfType<OkObjectResult>();
        
        OkObjectResult okObjectResult = (OkObjectResult) actionResult;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(_stationDtoStation1);
        
        actionResult = _stationController.GetStationByName(stationDtoStation2.NameStation);
        actionResult.Should().BeOfType<NotFoundObjectResult>();
        
        _stationController.AddStation(stationDtoStation2);
        
        actionResult = _stationController.GetStationByName(stationDtoStation2.NameStation);
        actionResult.Should().BeOfType<OkObjectResult>();
        
        okObjectResult = (OkObjectResult) actionResult;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(stationDtoStation2);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByNameErrorTest()
    {
        IActionResult actionResult = _stationController.GetStationByName("");
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByPositionTest()
    {
        StationDto stationDtoStation2 = new(){ NameStation = "Station2", 
            Position = new PositionDto { Latitude = 15.5, Longitude = 14.0 } };
        
        _stationController.AddStation(_stationDtoStation1);
        
        IActionResult actionResult = _stationController.GetStationByPosition(_stationDtoStation1.Position.Latitude, 
            _stationDtoStation1.Position.Longitude);
        actionResult.Should().BeOfType<OkObjectResult>();
        
        OkObjectResult okObjectResult = (OkObjectResult) actionResult;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(_stationDtoStation1);
        
        actionResult = _stationController.GetStationByPosition(stationDtoStation2.Position.Latitude, 
            stationDtoStation2.Position.Longitude);
        actionResult.Should().BeOfType<NotFoundObjectResult>();
        
        _stationController.AddStation(stationDtoStation2);
        
        actionResult = _stationController.GetStationByPosition(stationDtoStation2.Position.Latitude, 
            stationDtoStation2.Position.Longitude);
        actionResult.Should().BeOfType<OkObjectResult>();
        
        okObjectResult = (OkObjectResult) actionResult;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(stationDtoStation2);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetStationByPositionErrorTest()
    {
        IActionResult actionResult = _stationController.GetStationByPosition(91.0, _stationDtoStation1.Position.Longitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        
        actionResult = _stationController.GetStationByPosition(-91.0, _stationDtoStation1.Position.Longitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        
        actionResult = _stationController.GetStationByPosition(_stationDtoStation1.Position.Latitude, 180.01);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        
        actionResult = _stationController.GetStationByPosition(_stationDtoStation1.Position.Latitude, -180.01);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }
}