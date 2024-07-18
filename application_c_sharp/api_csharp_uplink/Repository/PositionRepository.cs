using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Repository;

public class PositionRepository(IGlobalInfluxDb globalInfluxDb) : IPositionRepository
{
    private const string MeasurementPosition = "positionCard";

    public PositionCard Add(PositionCard positionCard)
    {
        PositionCardDb positionCardDb = globalInfluxDb.Save(ConvertPositionCardToDb(positionCard)).Result;
        return ConvertDbToBus(positionCardDb);
    }
    

    public PositionCard? GetLast(string devEuiCard)
    {
        string query = "from(bucket: \"mybucket\")\n  |> range(start: -15m)\n  "
        + $"|> filter(fn: (r) => r._measurement == \"{MeasurementPosition}\" and r.devEuiCard == \"{devEuiCard}\")\n  " +
        "|> last()";
        
        List <PositionCardDb> positionCardDbs = globalInfluxDb.Get<PositionCardDb>(query).Result;
        return positionCardDbs.Count > 0 ? ConvertDbToBus(positionCardDbs[0]) : null;
    }
    
    private static PositionCardDb ConvertPositionCardToDb(PositionCard positionCard)
    {
        return new PositionCardDb
        {
            DevEuiCard = positionCard.DevEuiCard,
            Longitude = positionCard.Position.Longitude,
            Latitude = positionCard.Position.Latitude,
            Timestamp = DateTime.Now
        };
    }
    
    private static PositionCard ConvertDbToBus(PositionCardDb positionCardDb)
    {
        return new PositionCard(new Position(positionCardDb.Latitude, positionCardDb.Longitude), positionCardDb.DevEuiCard);
    }
}