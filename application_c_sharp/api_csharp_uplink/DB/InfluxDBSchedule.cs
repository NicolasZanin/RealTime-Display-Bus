
using api_csharp_uplink.Entities;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;

namespace api_csharp_uplink.DB
{
    public class InfluxDBSchedule(GlobalInfluxDb globalInfluxDb) : IInfluxDBSchedule
    {

        private readonly GlobalInfluxDb _globalInfluxDb = globalInfluxDb;
        public async Task<Schedule?> Add(Schedule schedule)
        {
            //TODO : Supprimer le point si la station existe deja

            var point1 = PointData.Measurement("bus_stations_infos")
             .Tag("station_name", schedule.name)
             .Field("latitude", schedule.latitude)
             .Field("longitude", schedule.longitude);
            int i = 0;
            foreach (var horaire in schedule.schedules)
            {
                if (i < 9)
                {
                    point1.Field("horaire_0" + (i + 1), horaire);
                }
                else
                {
                    point1.Field("horaire_" + (i + 1), horaire);
                }
            }

            try
            {
                await _globalInfluxDb.GetWriteApiAsync(point1);
                return schedule;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing to InfluxDB cloud: " + e.Message);
                return null;
            }
        }

        public async Task Delete(string query)
        {
        /*    try
            {
                string start = "1970-01-01T00:00:00Z";
                string stop = DateTime.UtcNow.ToString("o");
                var predicate = $"_measurement=\"bus_stations_infos\" AND station_name=\"{query}\"";
                await _globalInfluxDb.GetDeleteApiAsync().DeleteAsync(start, stop, predicate, "bucketStation");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleting data from InfluxDB cloud: " + e.Message);
            }*/
        }

        public async Task<List<Tuple<string, double, double>>> GetAllPosition()
        {
            List<Tuple<string, double, double>> list = [];
            string query = $"from(bucket: \"bucketStation\") |> range(start: -inf) |> filter(fn: (r) => r._measurement == \"bus_stations_infos\")";
            List<FluxTable> tables = await _globalInfluxDb.GetQueryApiAsync(query); ;

            foreach (FluxTable table in tables)
            {
                foreach (FluxRecord record in table.Records)
                {
                    list.Add(ConvertRecordToPosition(record));
                }
            }

            return list;
        }
        public static Tuple<string, double, double> ConvertRecordToPosition(FluxRecord record)
        {
            string stationName = record.Values["station_name"].ToString();
            double lat = (double)record.Values["latitude"];
            double longitude = (double)record.Values["longitude"];
            return new Tuple<string, double, double> (stationName, lat, longitude);
        }
        public static Schedule ConvertRecordToSchedule(FluxRecord record)
        {
            string pattern = "^\\d{1,2}h\\d{2}$";
            Regex regex = new Regex(pattern);
            string stationName = record.Values["station_name"].ToString();
            double lat = (double)record.Values["latitude"];
            double longi = (double)record.Values["longitude"];
            List<string> horaires = new List<string>();

            foreach (var kvp in record.Values)
            {
                string key = kvp.Key;
                if (regex.IsMatch(key))
                {
                    horaires.Add(kvp.Value.ToString());
                }
            }

            return new Schedule
            {
                name = stationName,
                latitude = lat,
                longitude = longi,
                schedules = horaires
            };
        }
        public async Task<List<Schedule>> GetAllSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();
            string query = $"from(bucket: \"bucketStation\") |> range(start: -inf) |> filter(fn: (r) => r._measurement == \"bus_stations_infos\")";
            List<FluxTable> tables = await _globalInfluxDb.GetQueryApiAsync(query); ;
            foreach (FluxTable table in tables)
            {
                foreach (FluxRecord record in table.Records)
                {
                    schedules.Add(ConvertRecordToSchedule(record));
                }
            }
            return schedules;
        }

        public async Task<Schedule?> GetScheduleByStationName(string station)
        {

            string query = $"from(bucket: \"bucketStation\") |> range(start: -inf) |> filter(fn: (r) => r._measurement == \"bus_stations_infos\") |> filter(fn: (r) => r.station_name == \"{station}\")";
            List<FluxTable> tables = await _globalInfluxDb.GetQueryApiAsync(query); ;
            foreach (FluxTable table in tables)
            {
                foreach (FluxRecord record in table.Records)
                {
                    return ConvertRecordToSchedule(record);
                }
            }
            return null;
        }

        public async Task<Tuple<string, double, double>?> GetPositionByStationName(string station)
        {
            
            string query = $"from(bucket: \"bucketStation\") |> range(start: -inf) |> filter(fn: (r) => r._measurement == \"bus_stations_infos\") |> filter(fn: (r) => r.station_name == \"{station}\")";

            List<FluxTable> tables = await _globalInfluxDb.GetQueryApiAsync(query);

            foreach (FluxTable table in tables)
            {
                foreach (FluxRecord record in table.Records)
                {
                    return ConvertRecordToPosition(record);
                }
            }
            return null;
        }
    }
}