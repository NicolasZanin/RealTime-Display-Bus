using api_csharp_uplink.Composant;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.Composant;

public class GraphComposantTest
{
    private static List<Connexion>? _connexionsForward;
    private static List<Connexion>? _connexionsBackward;
    private readonly IGraphItinerary graphItinerary = new GraphComposant();

    public GraphComposantTest()
    {
        List<Station> stations = GenerateConnexion.AddStations();

        var (stationTimeDistanceForward, stationTimeDistanceBackward) = GenerateConnexion.GetStationTimeDistance(stations);

        _connexionsForward = GenerateConnexion.AddConnexions(stationTimeDistanceForward, "FORWARD");
        _connexionsBackward = GenerateConnexion.AddConnexions(stationTimeDistanceBackward, "BACKWARD");
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public void GetIndexesStation()
    {
        if (_connexionsForward == null)
            Assert.False(true);
        
        LinkedList<Connexion> connexions = new LinkedList<Connexion>(_connexionsForward);
        int[] indexes = GraphComposant.GetIndexesStation(connexions, "Station1", "StationF2");
        Assert.Equal([0, 1], indexes);
        
        indexes = GraphComposant.GetIndexesStation(connexions, "Station1", "Station5");
        Assert.Equal([0, 4], indexes);
        
        indexes = GraphComposant.GetIndexesStation(connexions, "Station5", "Station1");
        Assert.Equal([4, 0], indexes);
        
        indexes = GraphComposant.GetIndexesStation(connexions, "Station1", "StationB2");
        Assert.Equal([0, -1], indexes);
        
        indexes = GraphComposant.GetIndexesStation(connexions, "StationB2", "Station1");
        Assert.Equal([-1, 0], indexes);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public void GetTimeItinerary()
    {
        if (_connexionsForward == null)
            Assert.False(true);
        
        LinkedList<Connexion> connexions = new LinkedList<Connexion>(_connexionsForward);
        int time = GraphComposant.GetTimeBetweenStations(connexions, 0, 1);
        Assert.Equal(5, time);
        
        time = GraphComposant.GetTimeBetweenStations(connexions, 0, 4);
        Assert.Equal(20, time);
        
        time = GraphComposant.GetTimeBetweenStations(connexions, 4, 0);
        Assert.Equal(0, time);
        
        time = GraphComposant.GetTimeBetweenStations(connexions, 0, 6);
        Assert.Equal(-1, time);
        
        time = GraphComposant.GetTimeBetweenStations(connexions, 0, -2);
        Assert.Equal(0, time);
        
        time = GraphComposant.GetTimeBetweenStations(null, 0, 1);
        Assert.Equal(-1, time);
        
        time = GraphComposant.GetTimeBetweenStations(connexions, 4, 4);
        Assert.Equal(0, time);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public async Task AddItineraryCard()
    {
        if (_connexionsForward == null || _connexionsBackward == null)
            Assert.False(true);
        
        Itinerary itineraryForward = new Itinerary(1, "FORWARD", _connexionsForward);
        Itinerary itineraryBackward = new Itinerary(1, "BACKWARD", _connexionsBackward);
        await graphItinerary.RegisterItineraryCard(itineraryForward);
        await graphItinerary.RegisterItineraryCard(itineraryBackward);
        
        int timeBetweenStation = await graphItinerary.GetItineraryTime(1, "Station1", "Station5");
        Assert.Equal(20, timeBetweenStation);
        
        timeBetweenStation = await graphItinerary.GetItineraryTime(1, "Station5", "Station1");
        Assert.Equal(20, timeBetweenStation);
        
        timeBetweenStation = await graphItinerary.GetItineraryTime(1, "StationF2", "StationF4");
        Assert.Equal(10, timeBetweenStation);
        
        timeBetweenStation = await graphItinerary.GetItineraryTime(1, "StationF4", "StationF2");
        Assert.Equal(10, timeBetweenStation);
        
        timeBetweenStation = await graphItinerary.GetItineraryTime(1, "StationF2", "StationB2");
        Assert.Equal(-1, timeBetweenStation);
        
        timeBetweenStation = await graphItinerary.GetItineraryTime(1, "Station1", "StationB2");
        Assert.Equal(5, timeBetweenStation);
        
        timeBetweenStation = await graphItinerary.GetItineraryTime(1, "StationB2", "Station1");
        Assert.Equal(5, timeBetweenStation);
        
        timeBetweenStation = await graphItinerary.GetItineraryTime(1, "Station1", "Station1");
        Assert.Equal(-1, timeBetweenStation);
        
        await Assert.ThrowsAsync<NotFoundException>(() => graphItinerary.GetItineraryTime(2, "Station1", "Station5"));
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public async Task DeleteItinerary()
    {
        if (_connexionsForward == null || _connexionsBackward == null)
            Assert.False(true);
        
        Itinerary itineraryForward = new Itinerary(1, "FORWARD", _connexionsForward);
        Itinerary itineraryBackward = new Itinerary(1, "BACKWARD", _connexionsBackward);
        await graphItinerary.RegisterItineraryCard(itineraryForward);
        await graphItinerary.RegisterItineraryCard(itineraryBackward);
        
        await graphItinerary.RemoveItineraryCard(1, Orientation.FORWARD);
        int time = await graphItinerary.GetItineraryTime(1, "Station1", "Station5");
        Assert.Equal(20, time); // The itinerary is still present in the backward orientation
        
        await graphItinerary.RemoveItineraryCard(1, Orientation.BACKWARD);
        
        await Assert.ThrowsAsync<NotFoundException>(() => graphItinerary.GetItineraryTime(1, "Station1", "Station5"));
        await Assert.ThrowsAsync<NotFoundException>(() => graphItinerary.RemoveItineraryCard(1, Orientation.FORWARD));
    }
}