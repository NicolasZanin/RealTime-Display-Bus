using api_csharp_uplink.Composant;
using api_csharp_uplink.Controllers;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Interface;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Controllers;

public class PositionControllerTest
{
    private readonly PositionController _positionController;
    private readonly PositionBusDto _positionBusDto15140 = new(){ DevEuiNumber = "0", 
        Position = new PositionDto { Latitude = 15.0, Longitude = 14.0 } };
    
    public PositionControllerTest()
    {
        IPositionRepository positionRepository = new DbTestPosition();
        PositionComposant positionComposant = new(positionRepository);
        _positionController = new PositionController(positionComposant);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void AddPositionTest()
    {
        IActionResult actionResult = _positionController.AddNewPosition(_positionBusDto15140);
        actionResult.Should().BeOfType<CreatedResult>();
        CreatedResult createdResult = (CreatedResult) actionResult;
        createdResult.Should().NotBeNull();
        createdResult.Value.Should().BeEquivalentTo(_positionBusDto15140);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void AddPositionErrorTest()
    {
        PositionBusDto positionBusDtoErrorLatitude = new() { DevEuiNumber = _positionBusDto15140.DevEuiNumber, 
            Position = new PositionDto { Latitude = 91.0, Longitude = _positionBusDto15140.Position.Longitude  } };
        
        IActionResult actionResult = _positionController.AddNewPosition(positionBusDtoErrorLatitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        
        positionBusDtoErrorLatitude.Position.Latitude = -91.0;
        actionResult = _positionController.AddNewPosition(positionBusDtoErrorLatitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        
        PositionBusDto positionBusDtoErrorLongitude = new() { DevEuiNumber = _positionBusDto15140.DevEuiNumber, 
            Position = new PositionDto { Latitude = _positionBusDto15140.Position.Latitude, Longitude = 180.01  } };
        
        actionResult = _positionController.AddNewPosition(positionBusDtoErrorLongitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
        
        positionBusDtoErrorLongitude.Position.Longitude = -180.01;
        actionResult = _positionController.AddNewPosition(positionBusDtoErrorLongitude);
        actionResult.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetPositionTest()
    {
        PositionBusDto positionBusDto15141 = new(){ DevEuiNumber = "1", 
            Position = new PositionDto { Latitude = 15.0, Longitude = 14.0 } };
        PositionBusDto positionBusDto14140 = new(){ DevEuiNumber = "0", 
            Position = new PositionDto { Latitude = 14.5, Longitude = 14.0 } };
        
        _positionController.AddNewPosition(_positionBusDto15140);
        _positionController.AddNewPosition(positionBusDto15141);
        
        IActionResult actionResult = _positionController.GetLastPositionByDevEuiNumber(_positionBusDto15140.DevEuiNumber);
        actionResult.Should().BeOfType<OkObjectResult>();
        
        OkObjectResult okObjectResult = (OkObjectResult) actionResult;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(_positionBusDto15140);
        
        actionResult = _positionController.GetLastPositionByDevEuiNumber(positionBusDto15141.DevEuiNumber);
        actionResult.Should().BeOfType<OkObjectResult>();
        
        okObjectResult = (OkObjectResult) actionResult;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(positionBusDto15141);
        
        _positionController.AddNewPosition(positionBusDto14140);
        
        actionResult = _positionController.GetLastPositionByDevEuiNumber(positionBusDto14140.DevEuiNumber);
        actionResult.Should().BeOfType<OkObjectResult>();
        
        okObjectResult = (OkObjectResult) actionResult;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(positionBusDto14140);
        
        actionResult = _positionController.GetLastPositionByDevEuiNumber("2");
        actionResult.Should().BeOfType<NotFoundObjectResult>();
    }
}