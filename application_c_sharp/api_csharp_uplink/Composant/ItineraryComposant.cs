using System.Collections.Concurrent;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using api_csharp_uplink.Repository.Interface;

namespace api_csharp_uplink.Composant;

public class ItineraryComposant(IItineraryRepository itineraryRepository, IStationFinder stationFinder) : 
    IItineraryRegister, IItineraryFinder
{
    public async Task<Itinerary> AddItinerary(int lineNumber, string orientation, List<ConnexionDto> connexions)
    {
        if (await itineraryRepository.FindItinerary(lineNumber, orientation) != null)
            throw new AlreadyCreateException($"Itinerary in line {lineNumber} and Orientation {orientation} already exist");

        List<string> connexionsSorted = TopologicalSort(connexions).AsParallel()
            .Where(connexion => connexion.Length > 0).ToList();
        
        List<Connexion> connexionsList = [..new Connexion[connexionsSorted.Count]];
        await Parallel.ForAsync(0, connexionsSorted.Count,
            async (indexConnexion, _) =>
            {
                Connexion connexion = await GetConnexions(lineNumber, orientation, connexionsSorted[indexConnexion]);
                connexionsList[indexConnexion] = connexion;
            });
        
        Itinerary itinerary = new Itinerary(lineNumber, orientation, connexionsList);
        return await itineraryRepository.AddItinerary(itinerary);
    }

    public async Task<Itinerary> FindItinerary(int lineNumber, string orientation)
    {
        if (lineNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(lineNumber), "Line number must be positive");
        Enum.Parse<Orientation>(orientation);
        
        return await itineraryRepository.FindItinerary(lineNumber, orientation) ?? 
               throw new NotFoundException($"Itinerary not found with line {lineNumber} and orientation {orientation}");
    }

    public async Task<Itinerary> FindItineraryBetweenStation(int lineNumber, string orientation, string station1, string station2)
    {
        if (lineNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(lineNumber), "Line number must be positive");
        Enum.Parse<Orientation>(orientation);
        
        if (string.IsNullOrEmpty(station1))
            throw new ArgumentNullException(nameof(station1), "Station1 must not be null or empty");
        if (string.IsNullOrEmpty(station2))
            throw new ArgumentNullException(nameof(station2), "Station2 must not be null or empty");
        
        return await itineraryRepository.FindItineraryBetweenStation(lineNumber, orientation, station1, station2) ??
               throw new NotFoundException($"Itinerary not found with line {lineNumber} and orientation {orientation}");
    }
    
    public Task<bool> DeleteItinerary(int lineNumber, string orientation)
    {
        if (lineNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(lineNumber), "Line number must be positive");
        Enum.Parse<Orientation>(orientation);
        
        return itineraryRepository.DeleteItinerary(lineNumber, orientation);
    }
    
    private static List<string> TopologicalSort(List<ConnexionDto> connexionDtos)
    {
        var graph = new ConcurrentDictionary<string, List<string>>();
        var inDegree = new ConcurrentDictionary<string, int>();

        Parallel.ForEach(connexionDtos, (connexionDto, _) =>
        {
            if (!graph.ContainsKey(connexionDto.CurrentNameStation))
                graph[connexionDto.CurrentNameStation] = [];
            if (!graph.ContainsKey(connexionDto.NextNameStation))
                graph[connexionDto.NextNameStation] = [];

            inDegree.TryAdd(connexionDto.CurrentNameStation, 0);
            inDegree.TryAdd(connexionDto.NextNameStation, 0);
        });

        foreach (ConnexionDto connexionDto in connexionDtos)
        {
            if (inDegree[connexionDto.NextNameStation] != 0) 
                continue;
            
            graph[connexionDto.CurrentNameStation].Add(connexionDto.NextNameStation);
            inDegree[connexionDto.NextNameStation]++;
        }
        
        var queue = new Queue<string>();
        
        Parallel.ForEach(inDegree, (keyValuePair, _) =>
        {
            if (keyValuePair.Value == 0)
                queue.Enqueue(keyValuePair.Key);
        });
        
        var result = new List<string>();
        while (queue.Count > 0)
        {
            string current = queue.Dequeue();
            result.Add(current);
            
            Parallel.ForEach(graph[current], (next, _) =>
            {
                inDegree[next]--;
                if (inDegree[next] == 0)
                    queue.Enqueue(next);
            });
        }

        return result;
    }
    
    private async Task<Connexion> GetConnexions(int lineNumber, string orientation, string nameStation)
    {
        Station station = await stationFinder.GetStation(nameStation);
        return new Connexion(lineNumber, orientation, station, 5, 5);
    }
}