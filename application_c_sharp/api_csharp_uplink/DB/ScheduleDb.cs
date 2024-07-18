using InfluxDB.Client.Core;

namespace api_csharp_uplink.DB;

[Measurement("schedule")]
public class ScheduleDb
{
    [Column("stationName", IsTag = true)]
    public string StationName { get; set; } = "";
    
    [Column("lineNumber", IsTag = true)]
    public int LineNumber { get; set; }
    
    [Column("orientation", IsTag = true)]
    public string Orientation { get; set; } = "";
    
    [Column("Value")]
    public string Value { get; init; } = "";
    
    [Column(IsTimestamp = true)]
    public DateTime Hours { get; set; }
}