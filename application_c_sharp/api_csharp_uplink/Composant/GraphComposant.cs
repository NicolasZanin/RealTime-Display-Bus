using System.Collections.Concurrent;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace api_csharp_uplink.Composant;

public class GraphComposant : IGraphPosition, IGraphItinerary
{
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private readonly ConcurrentDictionary<LineOrientation, LinkedList<Connexion>> _graph = new();
    
    public GraphComposant(int timeCache=300)
    {
         Console.WriteLine(_graph.Count);
    }
    public Task<int> RegisterPositionCard(Card card, Position position)
    {
        _cache.TryGetValue(card.DevEuiCard, out CardNearStation? value);
        
        if (value == null)
        {
            CardNearStation cardNewPosition = new(position);
            _cache.Set(card.DevEuiCard, cardNewPosition, TimeSpan.FromMinutes(300));
        }
        else
        {
            _cache.Remove(card.DevEuiCard);
            _cache.Set(card.DevEuiCard, value, TimeSpan.FromMinutes(300));
        }

        return Task.FromResult(300);
    }

    public Task RegisterItineraryCard(Itinerary itinerary)
    {
        LinkedList<Connexion> connexions = new(itinerary.connexions);
        LineOrientation lineOrientation = new(itinerary.lineNumber, itinerary.orientation);
        bool add = _graph.TryAdd(lineOrientation, connexions);
        if (add == false)
            throw new AggregateException();
        return Task.CompletedTask;
    }

    public Task<int> GetItineraryTime(int lineNumber,string nameStation1, string nameStation2)
    {
        LineOrientation lineOrientationForward = new(lineNumber, Orientation.FORWARD);
        LineOrientation lineOrientationBackward = new(lineNumber, Orientation.BACKWARD);
        
        bool existForward = _graph.TryGetValue(lineOrientationForward, out LinkedList<Connexion>? connexionsForward);
        bool existBackward = _graph.TryGetValue(lineOrientationBackward, out LinkedList<Connexion>? connexionsBackward);
        
        if (!existForward && !existBackward)
            throw new NotFoundException($"The line with lineNumber {lineNumber} not found");

        int[] indexesForward = GetIndexesStation(connexionsForward, nameStation1, nameStation2);
        int[] indexesBackward = GetIndexesStation(connexionsBackward, nameStation1, nameStation2);
        int time = -1;

        if (indexesForward[0] != -1 && indexesForward[1] != -1 && indexesBackward[0] != -1 && indexesBackward[1] != -1)
            time = indexesForward[0] < indexesForward[1] ? GetTimeBetweenStations(connexionsForward, indexesForward[0], indexesForward[1]) 
                : GetTimeBetweenStations(connexionsForward, indexesBackward[0], indexesBackward[1]);
        
        if (indexesForward[0] != -1 && indexesForward[1] != -1)
            time = indexesForward[0] < indexesForward[1] ? GetTimeBetweenStations(connexionsForward, indexesForward[0], indexesForward[1]) 
                : GetTimeBetweenStations(connexionsForward, indexesForward[1], indexesForward[0]);
        
        if (indexesBackward[0] != -1 && indexesBackward[1] != -1)
            time = indexesBackward[0] < indexesBackward[1] ? GetTimeBetweenStations(connexionsBackward, indexesBackward[0], indexesBackward[1]) 
                : GetTimeBetweenStations(connexionsBackward, indexesBackward[1], indexesBackward[0]);
            
        return Task.FromResult(time);
    }

    public static int[] GetIndexesStation(LinkedList<Connexion>? connexions, string nameStation1, string nameStation2)
    {
        if (connexions == null)
            return [-1, -1];
        
        LinkedListNode<Connexion>? linkedConnexions = connexions.First;
        int indexOne = -1;
        int indexTwo = -1;
        int indexCurrent = 0;
        
        while (linkedConnexions != null)
        {
            Station station = linkedConnexions.Value.stationCurrent;

            if (station.NameStation == nameStation1)
                indexOne = indexCurrent;
            else if (station.NameStation == nameStation2)
                indexTwo = indexCurrent;
            
            linkedConnexions = linkedConnexions.Next;
            indexCurrent += 1;
        }

        return [indexOne, indexTwo];
    }

    public Task RemoveItineraryCard(int lineNumber, Orientation orientation)
    {
        LineOrientation lineOrientation = new(lineNumber, orientation);
        
        if (!_graph.TryRemove(lineOrientation, out _))
            throw new NotFoundException($"The line with lineNumber {lineNumber} and orientation {orientation} not found");
        
        return Task.CompletedTask;
    }

    public static int GetTimeBetweenStations(LinkedList<Connexion>? connexions, int indexStation1, int indexStation2)
    {
        if (connexions == null || connexions.Count <= indexStation2)
            return -1;
        
        LinkedListNode<Connexion>? linkedConnexions = connexions.First;
        int indexCurrent = 0;
        int time = 0;
        
        while (linkedConnexions != null && indexCurrent < indexStation2)
        {
            if(indexCurrent >= indexStation1 && indexCurrent < indexStation2)
                time += linkedConnexions.Value.timeToNextStation;
            
            indexCurrent += 1;
            linkedConnexions = linkedConnexions.Next;
        }

        return time;
    }
}