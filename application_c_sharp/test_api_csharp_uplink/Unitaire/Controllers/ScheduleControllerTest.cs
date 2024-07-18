using api_csharp_uplink.Composant;
using api_csharp_uplink.Controllers;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Interface;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Controllers;

public class ScheduleControllerTest
{
    private readonly ScheduleController _scheduleController;
    private readonly HourDto _hourDto1010 = new(){Hour = 10, Minute = 10};
    private readonly HourDto _hourDto1020 = new(){Hour = 10, Minute = 20};
    
    public ScheduleControllerTest()
    {
        IScheduleRepository scheduleRepository = new DbTestSchedule();
        ScheduleComposant scheduleComposant = new ScheduleComposant(scheduleRepository);
        _scheduleController = new ScheduleController(scheduleComposant, scheduleComposant);
    }
    
    private void VerifyObjectResult<T>(ScheduleDto scheduleExpected, IActionResult result) where T : ObjectResult
    {
        result.Should().BeOfType<T>();
        T okObjectResult = (T) result;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(scheduleExpected);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestAddSchedule()
    {
        ScheduleDto scheduleExpected = new(){Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"};
        
        IActionResult result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010, _hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"});
        VerifyObjectResult<CreatedResult>(scheduleExpected, result);
        
        result = _scheduleController.GetScheduleForward("Station1", 1);
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
        
        scheduleExpected.Hours = [_hourDto1020];
        result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010, _hourDto1020], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"});
        VerifyObjectResult<CreatedResult>(scheduleExpected, result);
        
        result = _scheduleController.GetScheduleForward("Station1", 1);
        scheduleExpected.Hours.Add(_hourDto1010);
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
        
        scheduleExpected = new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "BACKWARD"};
        result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "BACKWARD"});
        VerifyObjectResult<CreatedResult>(scheduleExpected, result);
        
        result = _scheduleController.GetScheduleBackward("Station1", 1);
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
        
        result = _scheduleController.GetScheduleForward("Station1", 1);
        scheduleExpected = new ScheduleDto{Hours = [_hourDto1010, _hourDto1020], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"};
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
        
        scheduleExpected = new ScheduleDto{Hours = [_hourDto1010], LineNumber = 2, NameStation = "Station1", Orientation = "FORWARD"};
        result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 2, NameStation = "Station1", Orientation = "FORWARD"});
        VerifyObjectResult<CreatedResult>(scheduleExpected, result);
        
        scheduleExpected = new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station2", Orientation = "FORWARD"};
        result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station2", Orientation = "FORWARD"});
        VerifyObjectResult<CreatedResult>(scheduleExpected, result);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestAddScheduleError()
    {
        IActionResult result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"});
        result.Should().BeOfType<BadRequestObjectResult>();
        
        result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 0, NameStation = "Station1", Orientation = "FORWARD"});
        result.Should().BeOfType<BadRequestObjectResult>();
        
        result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "", Orientation = "FORWARD"});
        result.Should().BeOfType<BadRequestObjectResult>();
        
        result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "FORWAR"});
        result.Should().BeOfType<BadRequestObjectResult>();
        
        _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"});
        result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"});
        result.Should().BeOfType<BadRequestObjectResult>();
        
        result = _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "FORWAR"});
        result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFindSchedule()
    {
        _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"});
        
        ScheduleDto scheduleExpected = new(){Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"};
        IActionResult result = _scheduleController.GetScheduleForward("Station1", 1);
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
        
        _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1020], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"});
        scheduleExpected = new ScheduleDto{Hours = [_hourDto1010, _hourDto1020], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"};
        result = _scheduleController.GetScheduleForward("Station1", 1);
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
        
        _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "BACKWARD"});
        scheduleExpected = new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "BACKWARD"};
        result = _scheduleController.GetScheduleBackward("Station1", 1);
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
        
        result = _scheduleController.GetScheduleForward("Station1", 1);
        scheduleExpected = new ScheduleDto{Hours = [_hourDto1010, _hourDto1020], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"};
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
        
        _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 2, NameStation = "Station1", Orientation = "FORWARD"});
        scheduleExpected = new ScheduleDto{Hours = [_hourDto1010], LineNumber = 2, NameStation = "Station1", Orientation = "FORWARD"};
        result = _scheduleController.GetScheduleForward("Station1", 2);
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
        
        _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station2", Orientation = "FORWARD"});
        scheduleExpected = new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station2", Orientation = "FORWARD"};
        result = _scheduleController.GetScheduleForward("Station2", 1);
        VerifyObjectResult<OkObjectResult>(scheduleExpected, result);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFindScheduleError()
    {
        IActionResult result = _scheduleController.GetScheduleForward("", 1);
        result.Should().BeOfType<BadRequestObjectResult>();
        result = _scheduleController.GetScheduleForward("Station1", 0);
        result.Should().BeOfType<BadRequestObjectResult>();
        
        _scheduleController.AddSchedule(
            new ScheduleDto{Hours = [_hourDto1010], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"});
        result = _scheduleController.GetScheduleForward("Station1", 2);
        result.Should().BeOfType<NotFoundObjectResult>();
        
        result = _scheduleController.GetScheduleForward("Station2", 1);
        result.Should().BeOfType<NotFoundObjectResult>();
        
        result = _scheduleController.GetScheduleBackward("Station1", 1);
        result.Should().BeOfType<NotFoundObjectResult>();
    }
    
    private void VerifyObjectResultList<T>(List<ScheduleDto> schedulesExpected, IActionResult result) where T : ObjectResult
    {
        result.Should().BeOfType<T>();
        T okObjectResult = (T) result;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(schedulesExpected);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void TestFindScheduleByNameOrientation()
    {
        _scheduleController.AddSchedule(
            new ScheduleDto { Hours = [_hourDto1010, _hourDto1020], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD"});
        _scheduleController.AddSchedule(
            new ScheduleDto { Hours = [_hourDto1010, _hourDto1020], LineNumber = 2, NameStation = "Station1", Orientation = "FORWARD"});

        List<ScheduleDto> schedulesExpected = [
            new ScheduleDto{ Hours = [_hourDto1010, _hourDto1020], LineNumber = 1, NameStation = "Station1", Orientation = "FORWARD" },
            new ScheduleDto{ Hours = [_hourDto1010, _hourDto1020], LineNumber = 2, NameStation = "Station1", Orientation = "FORWARD" }
        ];
        IActionResult result = _scheduleController.GetScheduleByStationNameForward("Station1");
        VerifyObjectResultList<OkObjectResult>(schedulesExpected, result);
        
        
        result = _scheduleController.GetScheduleByStationNameForward("Station2");
        VerifyObjectResultList<OkObjectResult>([], result);
        
        result = _scheduleController.GetScheduleByStationNameBackward("Station1");
        VerifyObjectResultList<OkObjectResult>([], result);

        _scheduleController.AddSchedule(
            new ScheduleDto { Hours = [_hourDto1010, _hourDto1020], NameStation = "Station2", LineNumber = 1, Orientation = "FORWARD" });
        _scheduleController.AddSchedule(
            new ScheduleDto { Hours = [_hourDto1010, _hourDto1020], NameStation = "Station1", LineNumber = 1, Orientation = "BACKWARD" });
        
        schedulesExpected = [ new ScheduleDto{ Hours = [_hourDto1010, _hourDto1020], LineNumber = 1, NameStation = "Station2", Orientation = "FORWARD" } ];
        result = _scheduleController.GetScheduleByStationNameForward("Station2");
        VerifyObjectResultList<OkObjectResult>(schedulesExpected, result);
        
        schedulesExpected = [ new ScheduleDto{ Hours = [_hourDto1010, _hourDto1020], LineNumber = 1, NameStation = "Station1", Orientation = "BACKWARD" } ];
        result = _scheduleController.GetScheduleByStationNameBackward("Station1");
        VerifyObjectResultList<OkObjectResult>(schedulesExpected, result);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void TestFindScheduleByNameOrientationError()
    {
        IActionResult result = _scheduleController.GetScheduleByStationNameForward("");
        result.Should().BeOfType<BadRequestObjectResult>();
        
        result = _scheduleController.GetScheduleByStationNameBackward("");
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}