using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using System.Text.Json;

namespace api_csharp_uplink.DB;

public class InfluxDbPosition(GlobalInfluxDb globalInfluxDb) : IInfluxDbPosition
{
    private const string MeasurementPosition = "position";

    public async Task<PositionBus> Add(PositionBus positionBus)
    {
        var point = PointData.Measurement(MeasurementPosition)
            .Tag("DevEuiCard", positionBus.DevEuiNumber.ToString())
            .Field("Longitude", positionBus.Position.Longitude)
            .Field("Latitude", positionBus.Position.Latitude)
            .Timestamp(DateTime.Now, WritePrecision.Ns);
        try { 
            await globalInfluxDb.GetWriteApiAsync(point);
            return positionBus;
        } catch (Exception e)
        {
            throw new DbException($"Error adding position to InfluxDB: {e.Message}");
        }            
    }
    
    private static PositionBus ConvertRecordToPosition(FluxRecord record)
    {
        string devEuiCard =(string) record.GetValueByKey("DevEuiCard");
        double latitude = (double) record.GetValueByKey("Latitude");
        double longitude = (double) record.GetValueByKey("Longitude");
        PositionBus positionBus = new(new Position(latitude, longitude), int.Parse(devEuiCard));

        return positionBus;
    }
    
    private static PositionBus? GetPositionBus(List<FluxTable> list)
    {
        if (list.Count > 0 && list[0].Records.Count > 0) 
        {
            return ConvertRecordToPosition(list[0].Records[0]);
        }

        return null;
    }

    public async Task<PositionBus?> GetLast(int devEuiNumber)
    {
        string query = $"from(bucket: \"mybucket\")\n  " +
                        $"|> range(start: -15m)\n  " +
                        $"|> filter(fn: (r) => r._measurement == \"{MeasurementPosition}\" and r.DevEuiCard == \"{devEuiNumber}\")\n  " +
                        $"|> last()\n  " +
                        $"|> filter(fn: (r) => r._field == \"Latitude\" or r._field == \"Longitude\" or r._field == \"DevEuiCard\")\n  " +
                        $"|> pivot(rowKey:[\"_time\"], columnKey: [\"_field\"], valueColumn: \"_value\")\n  " +
                        $"|> keep(columns: [\"_time\", \"DevEuiCard\", \"Longitude\", \"Latitude\"])";
        try
        {
            List<FluxTable> list = await globalInfluxDb.GetQueryApiAsync(query);
            return GetPositionBus(list);
        }
        catch (Exception e)
        {
            throw new DbException("Error querying InfluxDB cloud: " + e.Message);
        }
    }
}