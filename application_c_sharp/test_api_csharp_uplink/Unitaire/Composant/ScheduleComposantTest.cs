using api_csharp_uplink.Composant;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Composant;

public class ScheduleComposantTest
{
    private readonly ScheduleComposant _scheduleComposant;
    private readonly DateTime _dateTime1010 = new(2024, 7, 15, 10, 10, 0, DateTimeKind.Utc);
    private readonly DateTime _dateTime1020 = new(2024, 7, 15, 10, 20, 0, DateTimeKind.Utc);
    
    public ScheduleComposantTest()
    {
        IScheduleRepository scheduleRepository = new DbTestSchedule();
        _scheduleComposant = new ScheduleComposant(scheduleRepository);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void AddScheduleTest()
    {
        Schedule scheduleExpected = new Schedule("Station1", 1, Orientation.FORWARD, [_dateTime1010]);
        
        Schedule scheduleActual = _scheduleComposant.AddSchedule("Station1", 1, "FORWARD", [_dateTime1010, _dateTime1010]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        scheduleActual = _scheduleComposant.FindSchedule("Station1", 1, Orientation.FORWARD);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        scheduleActual = _scheduleComposant.AddSchedule("Station1", 1, "FORWARD", [_dateTime1010, _dateTime1020]);
        scheduleExpected = new Schedule("Station1", 1, Orientation.FORWARD, [_dateTime1020]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        scheduleActual = _scheduleComposant.FindSchedule("Station1", 1, Orientation.FORWARD);
        scheduleExpected = new Schedule("Station1", 1, Orientation.FORWARD, [_dateTime1010, _dateTime1020]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        scheduleActual = _scheduleComposant.AddSchedule("Station1", 1, "BACKWARD", [_dateTime1010]);
        scheduleExpected = new Schedule("Station1", 1, Orientation.BACKWARD, [_dateTime1010]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        scheduleActual = _scheduleComposant.FindSchedule("Station1", 1, Orientation.BACKWARD);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        scheduleActual = _scheduleComposant.FindSchedule("Station1", 1, Orientation.FORWARD);
        scheduleExpected = new Schedule("Station1", 1, Orientation.FORWARD, [_dateTime1010, _dateTime1020]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        scheduleActual = _scheduleComposant.AddSchedule("Station1", 2, "FORWARD", [_dateTime1010]);
        scheduleExpected = new Schedule("Station1", 2, Orientation.FORWARD, [_dateTime1010]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        scheduleActual =  _scheduleComposant.AddSchedule("Station2", 1, "FORWARD", [_dateTime1010]);
        scheduleExpected = new Schedule("Station2", 1, Orientation.FORWARD, [_dateTime1010]);
        Assert.Equal(scheduleExpected, scheduleActual);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void AddErrorScheduleTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _scheduleComposant.AddSchedule("Station1", 1, "FORWARD", []));
        Assert.Throws<ArgumentOutOfRangeException>(() => _scheduleComposant.AddSchedule("Station1", 0, "FORWARD", [_dateTime1010]));
        Assert.Throws<ArgumentNullException>(() => _scheduleComposant.AddSchedule("", 1, "FORWARD", [_dateTime1010]));
        Assert.Throws<ArgumentException>(() => _scheduleComposant.AddSchedule("Station1", 1, "FORWAR", [_dateTime1010]));
       
        _scheduleComposant.AddSchedule("Station1", 1, "FORWARD", [_dateTime1010]);
        Assert.Throws<ArgumentOutOfRangeException>(() => _scheduleComposant.AddSchedule("Station1", 1, "FORWARD", [_dateTime1010]));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void FindScheduleTest()
    {
        _scheduleComposant.AddSchedule("Station1", 1, "FORWARD", [_dateTime1010]);
        
        Schedule scheduleExpected = new Schedule("Station1", 1, Orientation.FORWARD, [_dateTime1010]);
        Schedule scheduleActual = _scheduleComposant.FindSchedule("Station1", 1, Orientation.FORWARD);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        _scheduleComposant.AddSchedule("Station1", 1, "FORWARD", [_dateTime1020]);
        scheduleActual = _scheduleComposant.FindSchedule("Station1", 1, Orientation.FORWARD);
        scheduleExpected = new Schedule("Station1", 1, Orientation.FORWARD, [_dateTime1010, _dateTime1020]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        _scheduleComposant.AddSchedule("Station1", 1, "BACKWARD", [_dateTime1010]);
        scheduleActual = _scheduleComposant.FindSchedule("Station1", 1, Orientation.BACKWARD);
        scheduleExpected = new Schedule("Station1", 1, Orientation.BACKWARD, [_dateTime1010]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        scheduleActual = _scheduleComposant.FindSchedule("Station1", 1, Orientation.FORWARD);
        scheduleExpected = new Schedule("Station1", 1, Orientation.FORWARD, [_dateTime1010, _dateTime1020]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        _scheduleComposant.AddSchedule("Station1", 2, "FORWARD", [_dateTime1010]);
        scheduleActual = _scheduleComposant.FindSchedule("Station1", 2, Orientation.FORWARD);
        scheduleExpected = new Schedule("Station1", 2, Orientation.FORWARD, [_dateTime1010]);
        Assert.Equal(scheduleExpected, scheduleActual);
        
        _scheduleComposant.AddSchedule("Station2", 1, "FORWARD", [_dateTime1010]);
        scheduleActual = _scheduleComposant.FindSchedule("Station2", 1, Orientation.FORWARD);
        scheduleExpected = new Schedule("Station2", 1, Orientation.FORWARD, [_dateTime1010]);
        Assert.Equal(scheduleExpected, scheduleActual);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void FindErrorScheduleTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _scheduleComposant.FindSchedule("Station1", 0, Orientation.FORWARD));
        Assert.Throws<ArgumentNullException>(() => _scheduleComposant.FindSchedule("", 1, Orientation.FORWARD));
        
        _scheduleComposant.AddSchedule("Station1", 1, "FORWARD", [_dateTime1010]);
        Assert.Throws<NotFoundException>(() => _scheduleComposant.FindSchedule("Station1", 1, Orientation.BACKWARD));
        Assert.Throws<NotFoundException>(() => _scheduleComposant.FindSchedule("Station1", 2, Orientation.FORWARD));
        Assert.Throws<NotFoundException>(() => _scheduleComposant.FindSchedule("Station2", 1, Orientation.FORWARD));
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void FindScheduleByNameOrientationTest()
    {
        _scheduleComposant.AddSchedule("Station1", 1, "FORWARD", [_dateTime1010, _dateTime1020]);
        _scheduleComposant.AddSchedule("Station1", 2, "FORWARD", [_dateTime1010, _dateTime1020]);
        
        List<Schedule> schedulesExpected = [
            new Schedule("Station1", 1, Orientation.FORWARD, [_dateTime1010, _dateTime1020]),
            new Schedule("Station1", 2, Orientation.FORWARD, [_dateTime1010, _dateTime1020])
        ];
        List<Schedule> schedulesActual = _scheduleComposant.FindScheduleByStationNameOrientation("Station1", Orientation.FORWARD);
        Assert.Equal(schedulesExpected, schedulesActual);
        
        schedulesActual = _scheduleComposant.FindScheduleByStationNameOrientation("Station2", Orientation.FORWARD);
        Assert.Empty(schedulesActual);
        
        schedulesActual = _scheduleComposant.FindScheduleByStationNameOrientation("Station1", Orientation.BACKWARD);
        Assert.Empty(schedulesActual);
        
        _scheduleComposant.AddSchedule("Station2", 1, "FORWARD", [_dateTime1010, _dateTime1020]);
        _scheduleComposant.AddSchedule("Station1", 1, "BACKWARD", [_dateTime1010, _dateTime1020]);
        
        schedulesExpected = [new Schedule("Station1", 1, Orientation.BACKWARD, [_dateTime1010, _dateTime1020])];
        schedulesActual = _scheduleComposant.FindScheduleByStationNameOrientation("Station1", Orientation.BACKWARD);
        Assert.Equal(schedulesExpected, schedulesActual);
        
        schedulesExpected = [new Schedule("Station2", 1, Orientation.FORWARD, [_dateTime1010, _dateTime1020])];
        schedulesActual = _scheduleComposant.FindScheduleByStationNameOrientation("Station2", Orientation.FORWARD);
        Assert.Equal(schedulesExpected, schedulesActual);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void FindErrorScheduleByNameOrientationTest()
    {
        Assert.Throws<ArgumentNullException>(() => _scheduleComposant.FindScheduleByStationNameOrientation("", Orientation.FORWARD));
    }
}