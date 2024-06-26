using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Repository;

public class PositionRepository(GlobalInfluxDb globalInfluxDb) : IPositionRepository
{
    private const string MeasurementPosition = "positionCard";

    public PositionBus Add(PositionBus positionBus)
    {
        PositionCardDb positionCardDb = globalInfluxDb.Save(ConvertPositionCardToDb(positionBus)).Result;
        return ConvertDbToBus(positionCardDb);
    }
    

    public PositionBus? GetLast(string devEuiCard)
    {
        string query = "from(bucket: \"mybucket\")\n  |> range(start: -15m)\n  "
        + $"|> filter(fn: (r) => r._measurement == \"{MeasurementPosition}\" and r.devEuiCard == \"{devEuiCard}\")\n  " +
        "|> last()";
        
        List <PositionCardDb> positionCardDbs = globalInfluxDb.Get<PositionCardDb>(query).Result;
        return positionCardDbs.Count > 0 ? ConvertDbToBus(positionCardDbs[0]) : null;
    }
    
    private static PositionCardDb ConvertPositionCardToDb(PositionBus positionBus)
    {
        return new PositionCardDb
        {
            DevEuiCard = positionBus.DevEuiCard,
            Longitude = positionBus.Position.Longitude,
            Latitude = positionBus.Position.Latitude,
            Timestamp = DateTime.Now
        };
    }
    
    private static PositionBus ConvertDbToBus(PositionCardDb positionCardDb)
    {
        return new PositionBus(new Position(positionCardDb.Latitude, positionCardDb.Longitude), positionCardDb.DevEuiCard);
    }
}