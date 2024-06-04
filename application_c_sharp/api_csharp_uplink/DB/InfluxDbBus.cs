using api_csharp_uplink.Entities;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace api_csharp_uplink.DB
{
    public class InfluxDbBus(GlobalInfluxDb globalInfluxDb) : IInfluxDBBus
    {
        private static readonly string MeasurementBUS = "bus";
        private readonly GlobalInfluxDb _globalInfluxDb = globalInfluxDb;

        public async Task<Bus?> Add(Bus bus)
        {
            var point = PointData.Measurement(MeasurementBUS)
                .Tag("DevEUICard", bus.DevEUICard.ToString())
                .Tag("BusNumber", bus.BusNumber.ToString())
                .Field("LineBus", bus.LineBus)
                .Timestamp(DateTime.Now, WritePrecision.Ns);
            try { 
                await _globalInfluxDb.GetWriteApiAsync(point);
                return bus;
            } catch (Exception e)
            {
                Console.WriteLine("Error writing to InfluxDB cloud: " + e.Message);
                return null;
            }            
        }

        private static Bus ConvertRecordToBus(FluxRecord record)
        {
            string devEUICard = record.Values["DevEUICard"]?.ToString() ?? "0";
            string busNumber = record.Values["BusNumber"]?.ToString() ?? "0";
            long lineBus = (long)record.GetValue();

            Bus bus = new()
            {
                BusNumber = int.Parse(busNumber),
                DevEUICard = int.Parse(devEUICard),
                LineBus = (int)lineBus
            };

            return bus;
        }

        private static Bus? GetBus(List<FluxTable> list)
        {
            foreach (FluxTable table in list)
            {
                foreach (FluxRecord record in table.Records)
                {
                    return ConvertRecordToBus(record);
                }
            }

            return null;
        }

        public async Task<Bus?> GetByBusNumber(int BusNumber)
        {
            string query = $"from(bucket: \"mybucket\") |> range(start: 0) |> filter(fn: (r) => r._measurement == \"{MeasurementBUS}\") |> filter(fn: (r) => r.BusNumber == \"{BusNumber}\")";
            List<FluxTable> list;
            
            try
            {
                list = await _globalInfluxDb.GetQueryApiAsync(query);
            } catch (Exception e)
            {
                throw new Exception("Error querying InfluxDB cloud: " + e.Message);
            }

            return GetBus(list);
        }

        public async Task<Bus?> GetByDevEUI(int DevEUI)
        {
            string query = $"from(bucket: \"mybucket\") |> range(start: 0) |> filter(fn: (r) => r._measurement == \"{MeasurementBUS}\") |> filter(fn: (r) => r.DevEUICard == \"{DevEUI}\")";
            List<FluxTable> list;

            try
            {
                list = await _globalInfluxDb.GetQueryApiAsync(query);
            }
            catch (Exception e)
            {
                throw new Exception("Error querying InfluxDB cloud: " + e.Message);
            }

            return GetBus(list);
        }

        public Task Delete(string query)
        {
            throw new NotImplementedException();
        }


        public async Task<List<Bus>> GetAll()
        {
            List<Bus> list = [];
            string query = $"from(bucket: \"mybucket\") |> range(start: 0) |> filter(fn: (r) => r._measurement == \"{MeasurementBUS}\")";
            List<FluxTable> tables = await _globalInfluxDb.GetQueryApiAsync(query); ;
            
            foreach (FluxTable table in tables)
            {
                foreach (FluxRecord record in table.Records)
                {
                    list.Add(ConvertRecordToBus(record));
                }
            }

            return list;            
        }
    }
}
