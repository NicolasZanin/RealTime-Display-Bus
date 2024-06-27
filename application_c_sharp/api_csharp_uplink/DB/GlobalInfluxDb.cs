using api_csharp_uplink.DirException;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;

namespace api_csharp_uplink.DB;

public class GlobalInfluxDb(string token = "77m_AFWkRSDcMUmFv7_50IR2BVuZqcUvY_7w51hPHnUP9KmcW4TsY6U9vfww-EmLLwa6RS7rWjZ9sJLI1ZtzVw==")
{
    private readonly InfluxDBClient _client = new("http://influxdb:8086", token);
    private const string BucketName = "mybucket";
    private const string OrgName = "myorg";
    private const string BaseQuery = "from(bucket: \"mybucket\")\n  |> range(start: 0)\n";
    
    public async Task<T> Save<T>(T data)
    {
        try
        {
            await _client.GetWriteApiAsync().WriteMeasurementAsync(data, WritePrecision.Ms, BucketName, OrgName);
            return data;
        }
        catch (Exception e)
        {
            throw new DbException("Error writing to InfluxDB cloud: " + e.Message);
        }
    }

    public async Task<List<T>> GetAll<T>(string measurementName)
    {
        try
        {
            string query = BaseQuery + $"|> filter(fn: (r) => r._measurement == \"{measurementName}\")";
            return await _client.GetQueryApi().QueryAsync<T>(query, OrgName);
        }
        catch (Exception e)
        {
            throw new DbException("Error writing to InfluxDB cloud: " + e.Message);
        }
    }
    
    public async Task<List<T>> Get<T>(string query)
    {
        try
        {
            return await _client.GetQueryApi().QueryAsync<T>(query, OrgName);
        }
        catch (Exception e)
        {
            throw new DbException("Error writing to InfluxDB cloud: " + e.Message);
        }
    }

    public Task<List<T>> Get<T>(string measurementName, string predicate)
    {
        return Get<T>(BaseQuery + $"|> filter(fn: (r) => r._measurement == \"{measurementName}\")\n" + predicate);
    }

    public Task Delete(string predicate)
    {
        return Delete(predicate, DateTime.UnixEpoch, DateTime.Now);
    }

    public async Task Delete(string predicate, DateTime start, DateTime end)
    {
        try
        {
            await _client.GetDeleteApi().Delete(start, end, predicate, BucketName, OrgName);
        }
        catch (Exception e)
        {
            throw new DbException("Error writing to InfluxDB cloud: " + e.Message);
        }
    }
}
