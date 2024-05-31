using api_csharp_uplink.Entities;
using InfluxDB.Client;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace api_csharp_uplink.DB
{
    public class GlobalInfluxDb
    {
        public readonly InfluxDBClient _client;
        public readonly string _bucket;
        public readonly string _org;

        public GlobalInfluxDb()
        {
            _client = new InfluxDBClient("http://influxdb:8086", "XwdqDu_hrjZx0-Sr-oHKhBxutpDKRVl512L3NDIBJHA1Ttylt2ZiSuCfNr4s0QBju7ZthcvdXKiu5aB3bQCTAA==");
            _bucket = "mybucket";
            _org = "myorg";
        }

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
                Console.WriteLine("Null");
                throw new Exception("Error querying InfluxDB cloud: " + e.Message);
            }
        }
    }
}
