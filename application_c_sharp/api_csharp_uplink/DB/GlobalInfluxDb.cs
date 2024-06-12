﻿using api_csharp_uplink.DirException;
using InfluxDB.Client;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace api_csharp_uplink.DB
{
    public class GlobalInfluxDb(string token = "77m_AFWkRSDcMUmFv7_50IR2BVuZqcUvY_7w51hPHnUP9KmcW4TsY6U9vfww-EmLLwa6RS7rWjZ9sJLI1ZtzVw==")
    {
        private readonly InfluxDBClient _client = new("http://influxdb:8086", token);
        private readonly string _bucket = "mybucket";
        private readonly string _org = "myorg";

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
                throw new DbException("Error querying InfluxDB cloud: " + e.Message);
            }
        }
    }
}
