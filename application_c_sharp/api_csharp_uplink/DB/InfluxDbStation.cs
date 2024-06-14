using System.Globalization;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace api_csharp_uplink.DB;

public class InfluxDbStation(GlobalInfluxDb globalInfluxDb) : IInfluxDbStation
{
    private const string MeasurementStation = "station";
    public async Task<Station> Add(Station station)
    {
        var point = PointData.Measurement(MeasurementStation)
            .Tag("NameStation", station.NameStation)
            .Tag("Longitude", station.Position.Longitude.ToString(CultureInfo.CurrentCulture))
            .Tag("Latitude", station.Position.Latitude.ToString(CultureInfo.CurrentCulture))
            .Field("Blank", "Blank")
            .Timestamp(DateTime.Now, WritePrecision.Ns);
        
        try { 
            await globalInfluxDb.GetWriteApiAsync(point);
            return station;
        } catch (Exception e)
        {
            throw new DbException($"Error adding position to InfluxDB: {e.Message}");
        }
    }

    public async Task<Station?> GetStation(string nameStation)
    {
        string query = $"from(bucket: \"mybucket\")\n  " +
                       $"|> range(start: 0)\n  " +
                       $"|> filter(fn: (r) => r._measurement == \"{MeasurementStation}\" and r.NameStation == \"{nameStation}\")";
        try
        {
            List<FluxTable> list = await globalInfluxDb.GetQueryApiAsync(query);
            return GetStation(list);
        }
        catch (Exception e)
        {
            throw new DbException("Error querying InfluxDB cloud: " + e.Message);
        }
    }

    public async Task<Station?> GetStation(Position position)
    {
        string query = $"from(bucket: \"mybucket\")\n  " +
                       $"|> range(start: 0)\n  " +
                       $"|> filter(fn: (r) => r._measurement == \"{MeasurementStation}\" and r.Longitude == \"{position.Longitude}\" and r.Latitude == \"{position.Latitude}\")";
        try
        {
            List<FluxTable> list = await globalInfluxDb.GetQueryApiAsync(query);
            return GetStation(list);
        }
        catch (Exception e)
        {
            throw new DbException("Error querying InfluxDB cloud: " + e.Message);
        }
    }        
    
    private static Station ConvertRecordToStation(FluxRecord record)
    {
        string nameStation =(string) record.Values["NameStation"];
        double latitude = double.Parse(record.Values["Latitude"].ToString() ?? "0");
        double longitude = double.Parse(record.Values["Longitude"].ToString() ?? "0");
        
        Station station = new(new Position(latitude, longitude), nameStation);
        return station;
    }
    
    private static Station? GetStation(List<FluxTable> list)
    {
        if (list.Count > 0 && list[0].Records.Count > 0) 
        {
            return ConvertRecordToStation(list[0].Records[0]);
        }

        return null;
    }
}