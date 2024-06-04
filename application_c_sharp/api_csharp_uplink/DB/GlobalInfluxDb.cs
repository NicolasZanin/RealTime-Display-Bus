using InfluxDB.Client;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace api_csharp_uplink.DB
{
    public class GlobalInfluxDb(string token = "XwdqDu_hrjZx0-Sr-oHKhBxutpDKRVl512L3NDIBJHA1Ttylt2ZiSuCfNr4s0QBju7ZthcvdXKiu5aB3bQCTAA==")
    {
        public readonly InfluxDBClient _client = new("http://influxdb:8086", token);
        public readonly string _bucket = "mybucket";
        public readonly string _org = "myorg";

        public Task GetWriteApiAsync(PointData pointData)
        {
            return _client.GetWriteApiAsync().WritePointAsync(pointData, _bucket, _org);
        }

        public Task<List<FluxTable>> GetQueryApiAsync(string query)
        {
            try {
                return _client.GetQueryApi().QueryAsync(query, _org);
            } 
            catch (Exception e)
            {
                throw new Exception("Error querying InfluxDB cloud: " + e.Message);
            }
        }
    }
}
