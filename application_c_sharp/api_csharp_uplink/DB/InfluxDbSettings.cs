namespace api_csharp_uplink.DB;

public class InfluxDbSettings
{
    public string Url { get; set; }
    public string Token { get; set; }
    public string OrgName { get; set; }
    public string BucketName { get; set; }
}