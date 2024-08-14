using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IItineraryRegister
{
    public Task<Itinerary> AddItinerary(int lineNumber, string orientation, List<Connexion> connexions);
    public Task<bool> DeleteItinerary(int lineNumber, string orientation);
}