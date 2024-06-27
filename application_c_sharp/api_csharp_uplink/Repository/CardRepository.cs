using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Repository;

public class CardRepository(GlobalInfluxDb globalInfluxDb) : ICardRepository
{
    private const string MeasurementCard = "card";

    public Card Add(Card card) {
        Task<CardDb> taskCardDb = globalInfluxDb.Save(ConvertCardToDb(card));
        return ConvertDbToCard(taskCardDb.Result);          
    }
    

    public Card? GetByDevEui(string devEuiCard)
    {
        string query = $"   |> filter(fn: (r) => r.devEuiCard == \"{devEuiCard}\")";
        List<CardDb> list = globalInfluxDb.Get<CardDb>(MeasurementCard, query).Result;
        return list.Count > 0 ? ConvertDbToCard(list[0]) : null;
    }
    
    public Card Modify(Card card)
    {
        string predicate = $"|> filter(fn: (r) => r.devEuiCard == \"{card.DevEuiCard}\")";
        globalInfluxDb.Delete(predicate);
        
        Task<CardDb> taskCardDb = globalInfluxDb.Save(ConvertCardToDb(card));
        return ConvertDbToCard(taskCardDb.Result);          
    }

    public List<Card> GetAll()
    {
        List<CardDb> cardDbs = globalInfluxDb.GetAll<CardDb>(MeasurementCard).Result;
        return cardDbs.Select(ConvertDbToCard).ToList();           
    }
    
    private static CardDb ConvertCardToDb(Card card)
    {
        return new CardDb
        {
            DevEuiCard = card.DevEuiCard,
            LineBus = card.LineBus,
            Timestamp = DateTime.Now
        };
    }
    
    private static Card ConvertDbToCard(CardDb cardDb)
    {
        return new Card(cardDb.DevEuiCard, cardDb.LineBus);
    }
}