using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace api_csharp_uplink.DB
{
    public class GlobalInfluxDb
    {
        public readonly InfluxDBClient _client;
        public readonly string _bucket = "mybucket";
        public readonly string _org = "myorg";

        public GlobalInfluxDb(string token = "XwdqDu_hrjZx0-Sr-oHKhBxutpDKRVl512L3NDIBJHA1Ttylt2ZiSuCfNr4s0QBju7ZthcvdXKiu5aB3bQCTAA==")
        {
            _client = new InfluxDBClient("http://influxdb:8086", token);
            
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                InitTest();
            }
        }

        private async void InitTest()
        {
            string bucketName = "mybucket";
            BucketsApi bucketsApi = _client.GetBucketsApi();
            Bucket existingBucket = await bucketsApi.FindBucketByNameAsync(bucketName);

            // Supprimer le bucket s'il existe
            if (existingBucket != null)
            {
                await bucketsApi.DeleteBucketAsync(existingBucket);
                Console.WriteLine($"Bucket '{bucketName}' supprimé.");
            }

            // Créer un nouveau bucket (base de données vide)
            BucketRetentionRules retentionRules =  new(BucketRetentionRules.TypeEnum.Expire, 0);
            await bucketsApi.CreateBucketAsync(bucketName, retentionRules, "3bda3be9a11ec317");
            Console.WriteLine($"Bucket '{bucketName}' créé.");
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
                throw new Exception("Error querying InfluxDB cloud: " + e.Message);
            }
        }
    }
}
