using InfluxDB.Client.Core;

namespace api_csharp_uplink.DB;

[Measurement("link")]
public class LinkDb
{
    [Column("nameStation1", IsTag = true)] public string NameStation1 { get; init; } = "";

    [Column("nameStation2", IsTag = true)] public string NameStation2 { get; init; } = "";

    [Column("lineNumber", IsTag = true)] public int LineNumber { get; init; }

    [Column("orientation", IsTag = true)] public string Orientation { get; init; } = "";

    [Column("distance", IsTag = true)] public double Distance { get; init; }

    [Column("seconds")] public int Seconds { get; init; }

    [Column(IsTimestamp = true)] public DateTime Time { get; init; } = DateTime.UtcNow;
}