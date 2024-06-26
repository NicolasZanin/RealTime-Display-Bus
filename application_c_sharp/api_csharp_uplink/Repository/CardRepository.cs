using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Repository;

public class CardRepository(GlobalInfluxDb globalInfluxDb) : ICardRepository
{
    private const string MeasurementCard = "card";

    public Bus Add(Bus bus) {
        Task<CardDb> taskCardDb = globalInfluxDb.save(ConvertCardToDb(bus));
        return ConvertDbToBus(taskCardDb.Result);          
    }
    

    public Bus? GetByDevEui(string devEuiCard)
    {
        string query = $"   |> filter(fn: (r) => r.devEuiCard == \"{devEuiCard}\")";
        List<CardDb> list = globalInfluxDb.get<CardDb>(MeasurementCard, query).Result;
        Console.WriteLine(list.Count);
        return list.Count > 0 ? ConvertDbToBus(list[0]) : null;
    }

    public List<Bus> GetAll()
    {
        List<CardDb> cardDbs = globalInfluxDb.getAll<CardDb>(MeasurementCard).Result;
        return cardDbs.Select(ConvertDbToBus).ToList();           
    }
    
    private static CardDb ConvertCardToDb(Bus bus)
    {
        return new CardDb
        {
            DevEuiCard = bus.DevEuiCard,
            BusNumber = bus.BusNumber,
            LineBus = bus.LineBus,
            Timestamp = DateTime.Now
        };
    }
    
    private static Bus ConvertDbToBus(CardDb cardDb)
    {
        return new Bus(cardDb.BusNumber, cardDb.DevEuiCard, cardDb.LineBus);
    }
}