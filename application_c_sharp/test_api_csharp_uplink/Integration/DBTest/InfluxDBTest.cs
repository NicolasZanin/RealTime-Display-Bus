
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;

namespace test_api_csharp_uplink.Integration.DBTest
{
    public class InfluxDBTest
    {
        public readonly InfluxDBClient Client;
        private readonly string _organizationID;

        public InfluxDBTest()
        {
            Client = new InfluxDBClient("http://influxdb:8086", "sOTUwWum_14-STWkMQQyO_ekLheUujU9ZO_boc1qQgoRRUVJGUIOpN9PYGVCYdc-l_3oCetWkNq3RFK69yMhiQ==");
            _organizationID = "052efff775d12db7";
        }


        public async Task InitializeBucket()
        {
            BucketsApi bucketsApi = Client.GetBucketsApi();
            List<Bucket> buckets = await bucketsApi.FindBucketsAsync();

            foreach(Bucket bucket in buckets)
            {
                if (!bucket.Name.StartsWith('_'))
                {
                    await bucketsApi.DeleteBucketAsync(bucket);
                    await bucketsApi.CreateBucketAsync(bucket.Name, _organizationID);
                }
            }
        }
    }
}
