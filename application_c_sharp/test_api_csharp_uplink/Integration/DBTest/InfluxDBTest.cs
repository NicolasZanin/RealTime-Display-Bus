
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;

namespace test_api_csharp_uplink.Integration.DBTest
{
    public class InfluxDBTest
    {
        public readonly InfluxDBClient Client;
        private readonly string _organizationID;
        private readonly DeleteApi _deleteApi;

        public InfluxDBTest()
        {
            Client = new InfluxDBClient("http://influxdb:8086", "mNxnpUdxk7h6z8GOchqIL7AM8au7Zt3y9uXX_jz9OXhEdi0qnOkLc3ZjWqW5rSc-ASVLafSF0xk_-IIWxir78A==");
            _organizationID = "7676f3c1acc9cda6";
            _deleteApi = Client.GetDeleteApi();
        }


        public async Task InitializeBucket()
        {
            await _deleteApi.Delete(DateTime.UnixEpoch, DateTime.UtcNow, "", "mybucket", _organizationID);
        }
    }
}
