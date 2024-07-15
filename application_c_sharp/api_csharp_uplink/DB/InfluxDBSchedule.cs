
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

        public  Task Delete(string query)
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
            return null;
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
        public static Schedule ConvertFluxTablesToSchedule(List<FluxTable> tables)
        {
            string stationName = "";
            double lat = 0;
            double longi = 0;
            List<string> horaires = new List<string>();
            foreach (FluxTable table in tables)
            {
                foreach (FluxRecord record in table.Records)
                {
                    stationName = record.Values["station_name"].ToString();

                    if (record.Values.ContainsValue("latitude"))
                    { lat = Convert.ToDouble(record.Values["latitude"]); }

                    if (record.Values.ContainsKey("longitude"))
                    { longi = Convert.ToDouble(record.Values["longitude"]); }
                    else
                    {
                        foreach (var kvp in record.Values)
                        { horaires.Add(kvp.Value.ToString()); }
                    }
                }
            }
            return new Schedule
            { name = stationName, latitude = lat, longitude = longi, schedules = horaires };
        }
        
        public static List<Schedule> ConvertFluxTablesToScheduleList(List<FluxTable> tables)
        {
            
            string pattern = "^\\d{1,2}h\\d{2}$";
            Regex regex = new Regex(pattern);
            string stationName = "";
            string currentStationName = "";
            double lat = 0;
            double longi = 0;
            List<string> horaires = new List<string>();
            List<Schedule> schedules = new List<Schedule>();
            foreach (FluxTable table in tables)
            {
                foreach (FluxRecord record in table.Records)
                {
                    stationName = record.Values["station_name"].ToString();
                    if (currentStationName != stationName && currentStationName != "")
                    {
                        schedules.Add(new Schedule
                        {
                            name = currentStationName,
                            latitude = lat,
                            longitude = longi,
                            schedules = horaires
                        });
                        horaires = new List<string>();
                       
                    }
                    currentStationName = stationName;
                    if (record.Values.ContainsKey("latitude"))
                    {
                        
                        lat = Convert.ToDouble(record.Values["latitude"]);
                    }

                    if (record.Values.ContainsKey("longitude"))
                    {
                        longi = Convert.ToDouble(record.Values["longitude"]);
                    }
                    else
                    {
                        foreach (var kvp in record.Values)
                        {
                            string key = kvp.Value.ToString();
                            if (regex.IsMatch(key))
                            {
                                horaires.Add(kvp.Value.ToString());
                            }
                        }
                    }
                }
            }

            //horaires.Add(record.Values.ToArray().ToString());
            return schedules;
            
        }
        public async Task<List<Schedule>> GetAllSchedulesAller()
        {
            List<Schedule> schedules = new List<Schedule>();
            string query = $"from(bucket: \"bucketStation\") |> range(start: -inf) |> filter(fn: (r) => r._measurement == \"bus_stations_infosAller\")";
            List<FluxTable> tables = await _globalInfluxDb.GetQueryApiAsync(query);
            return ConvertFluxTablesToScheduleList(tables);
        }
        public async Task<List<Schedule>> GetAllSchedulesRetour()
        {
            List<Schedule> schedules = new List<Schedule>();
            string query = $"from(bucket: \"bucketStation\") |> range(start: -inf) |> filter(fn: (r) => r._measurement == \"bus_stations_infosRetour\")";
            List<FluxTable> tables = await _globalInfluxDb.GetQueryApiAsync(query);
            return ConvertFluxTablesToScheduleList(tables);
        }
        public async Task<Schedule?> GetScheduleAllerByStationName(string station)
        {
            string query = $"from(bucket: \"bucketStation\") " +
                           $"|> range(start: -inf) " +
                           $"|> filter(fn: (r) => r._measurement == \"bus_stations_infosAller\") " +
                           $"|> filter(fn: (r) => r.station_name == \"{station}\")";
            List<FluxTable>  tables= await _globalInfluxDb.GetQueryApiAsync(query);
            return ConvertFluxTablesToSchedule(tables);
        }
        public async Task<Schedule?> GetScheduleRetourByStationName(string station)
        {

            string query = $"from(bucket: \"bucketStation\") |> range(start: -inf) |> filter(fn: (r) => r._measurement == \"bus_stations_infosRetour\") |> filter(fn: (r) => r.station_name == \"{station}\")";
            List<FluxTable>  tables= await _globalInfluxDb.GetQueryApiAsync(query);
          
            return ConvertFluxTablesToSchedule(tables);
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