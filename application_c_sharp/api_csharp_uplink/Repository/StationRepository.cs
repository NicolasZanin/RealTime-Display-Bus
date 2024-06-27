using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using api_csharp_uplink.DirException;

namespace api_csharp_uplink.Repository;

public class StationRepository(IGlobalInfluxDb globalInfluxDb) : IStationRepository
{
    private const string MeasurementStation = "station";
    
    public Station Add(Station station)
    {
        StationDb stationDb = globalInfluxDb.Save(ConvertStationToDb(station)).Result;
        return ConvertDbToStation(stationDb);
    }

    public Station? GetStation(string nameStation)
    {
        string query = $"|> filter(fn: (r) => r.nameStation == \"{nameStation}\")";
        try
        {
            List<StationDb> list = globalInfluxDb.Get<StationDb>(MeasurementStation, query).Result;
            return list.Count > 0 ? ConvertDbToStation(list[0]) : null;
        }
        catch (Exception e)
        {
            throw new DbException("Error querying InfluxDB cloud: " + e.Message);
        }
    }

    public Station? GetStation(Position position)
    {
        string query = $"  |> filter(fn: (r) => r.longitude == \"{position.Longitude}\" and r.latitude == \"{position.Latitude}\")";
        try
        {
            List<StationDb> list = globalInfluxDb.Get<StationDb>(MeasurementStation, query).Result;
            return list.Count > 0 ? ConvertDbToStation(list[0]) : null;
        }
        catch (Exception e)
        {
            throw new DbException("Error querying InfluxDB cloud: " + e.Message);
        }
    }        
    
    private static StationDb ConvertStationToDb(Station station)
    {
        return new StationDb
        {
            NameStation = station.NameStation,
            Longitude = station.Position.Longitude,
            Latitude = station.Position.Latitude,
            Timestamp = DateTime.Now
        };
    }
    
    private static Station ConvertDbToStation(StationDb stationDb)
    {
        return new Station(new Position(stationDb.Latitude, stationDb.Longitude), stationDb.NameStation);
    }
}