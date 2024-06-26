using api_csharp_uplink.DirException;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;

namespace api_csharp_uplink.DB
{
    public class GlobalInfluxDb(string token = "77m_AFWkRSDcMUmFv7_50IR2BVuZqcUvY_7w51hPHnUP9KmcW4TsY6U9vfww-EmLLwa6RS7rWjZ9sJLI1ZtzVw==")
    {
        private readonly InfluxDBClient _client = new("http://influxdb:8086", token);
        private readonly string _bucket = "mybucket";
        private readonly string _org = "myorg";
        private readonly string _baseQuery = "from(bucket: \"mybucket\")\n  |> range(start: 0)\n";
        
        public async Task<T> save<T>(T data)
        {
            try
            {
                await _client.GetWriteApiAsync().WriteMeasurementAsync(data, WritePrecision.Ms, _bucket, _org);
                return data;
            }
            catch (Exception e)
            {
                throw new DbException("Error writing to InfluxDB cloud: " + e.Message);
            }
        }

        public async Task<List<T>> getAll<T>(string measurementName)
        {
            try
            {
                string query = _baseQuery + $"|> filter(fn: (r) => r._measurement == \"{measurementName}\")";
                
                Console.WriteLine(typeof(T).Attributes.ToString());
                List<T> list = await _client.GetQueryApi().QueryAsync<T>(query, _org);
                return list;
            }
            catch (Exception e)
            {
                throw new DbException("Error writing to InfluxDB cloud: " + e.Message);
            }
        }
        
        public async Task<List<T>> get<T>(string query)
        {
            try
            {
                List<T> list = await _client.GetQueryApi().QueryAsync<T>(query, _org);
                return list;
            }
            catch (Exception e)
            {
                throw new DbException("Error writing to InfluxDB cloud: " + e.Message);
            }
        }

        public Task<List<T>> get<T>(string measurementName, string predicate)
        {
            return get<T>(_baseQuery + $"|> filter(fn: (r) => r._measurement == \"{measurementName}\")\n" + predicate);
        }

        public Task delete<T>(string predicate)
        {
            return delete<T>(predicate, DateTime.UnixEpoch, DateTime.Now);
        }

        public async Task delete<T>(string predicate, DateTime start, DateTime end)
        {
            try
            {
                await _client.GetDeleteApi().Delete(start, end, predicate, _bucket, _org);
            }
            catch (Exception e)
            {
                throw new DbException("Error writing to InfluxDB cloud: " + e.Message);
            }
        }
    }
}
