using api_csharp_uplink.DB;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace api_csharp_uplink.Repository;

public class CardRepository(GlobalInfluxDb globalInfluxDb) : ICardRepository
{
    private const string MeasurementCard = "card";

    public async Task<Bus?> Add(Bus bus)
    {
        var point = PointData.Measurement(MeasurementCard)
            .Tag("DevEuiCard", bus.DevEuiCard)
            .Tag("BusNumber", bus.BusNumber.ToString())
            .Field("LineBus", bus.LineBus)
            .Timestamp(DateTime.Now, WritePrecision.Ns);
        try { 
            await globalInfluxDb.GetWriteApiAsync(point);
            return bus;
        } catch (Exception e)
        {
            Console.WriteLine("Error writing to InfluxDB cloud: " + e.Message);
            return null;
        }            
    }

    private static Bus? GetBus(List<FluxTable> list)
    {
        if (list.Count > 0 && list[0].Records.Count > 0) 
        {
            return ConvertRecordToBus(list[0].Records[0]);
        }

        return null;
    }

    public async Task<Bus?> GetByDevEui(string devEuiCard)
    {
        string query = $"from(bucket: \"mybucket\") |> range(start: 0) |> filter(fn: (r) => r._measurement == \"{MeasurementCard}\") |> filter(fn: (r) => r.DevEuiCard == \"{devEuiCard}\")";
        List<FluxTable> list;

        try
        {
            list = await globalInfluxDb.GetQueryApiAsync(query);
        }
        catch (Exception e)
        {
            throw new DbException("Error querying InfluxDB cloud: " + e.Message);
        }

        return GetBus(list);
    }

    public async Task<List<Bus>> GetAll()
    {
        List<Bus> list = [];
        string query = $"from(bucket: \"mybucket\") |> range(start: 0) |> filter(fn: (r) => r._measurement == \"{MeasurementCard}\")";
        List<FluxTable> tables = await globalInfluxDb.GetQueryApiAsync(query);
        
        foreach (FluxTable table in tables)
        {
            foreach (FluxRecord record in table.Records)
            {
                list.Add(ConvertRecordToBus(record));
            }
        }

        return list;            
    }
    
    private static Bus ConvertRecordToBus(FluxRecord record)
    {
        string devEuiCard = record.Values["DevEuiCard"]?.ToString() ?? "0";
        string busNumber = record.Values["BusNumber"]?.ToString() ?? "0";
        long lineBus = (long)record.GetValue();

        Bus bus = new(int.Parse(busNumber), devEuiCard, (int) lineBus);
        return bus;
    }
}