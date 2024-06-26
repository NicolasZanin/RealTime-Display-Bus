using InfluxDB.Client.Core;

namespace api_csharp_uplink.DB;

[Measurement("card")]
public class CardDb
{
    [Column("busNumber", IsTag = true)]
    public int BusNumber { get; set; }
    
    [Column("devEuiCard", IsTag = true)]
    public string DevEuiCard { get; set; } = "";
    
    [Column("lineBus", IsTag = true)]
    public int LineBus { get; init; }
    
    [Column("value")]
    public string Value { get; set; } = "";
    
    [Column(IsTimestamp = true)]
    public DateTime Timestamp { get; set; }
}