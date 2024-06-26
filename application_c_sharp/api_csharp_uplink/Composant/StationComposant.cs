using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Composant;

public class StationComposant(IStationRepository stationRepository) : IStationComposant
{
    public Station AddStation(double latitude, double longitude, string nameStation)
    { 
        if (stationRepository.GetStation(nameStation) != null)
            throw new AlreadyCreateException($"Station {nameStation} already created.");

        Station station = new Station(new Position(latitude, longitude), nameStation);
        return stationRepository.Add(station);
    }

    public Station GetStation(string nameStation)
    {
        if (string.IsNullOrEmpty(nameStation))
            throw new ArgumentNullException(nameof(nameStation), "Name of station must not be null or empty.");
        
        return stationRepository.GetStation(nameStation) ?? 
               throw new NotFoundException($"The station with the name {nameStation} does not exist.");
    }

    public Station GetStation(double latitude, double longitude)
    {
        Position position = new Position(latitude, longitude);
        
        return stationRepository.GetStation(position) ??
               throw new NotFoundException($"The station with the Position {position} does not exist.");
    }
}