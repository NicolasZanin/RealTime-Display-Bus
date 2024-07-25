using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Composant;

// TODO : Add graph + load all links when the service is started
public class TimeEngine(ILinkRegistration linkRegistration, IStationFinder stationFinder) : IAddLink
{
    public async Task<Link> AddLink(string nameStation1, string nameStation2, int lineNumber, string orientation)
    {
        if (lineNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(lineNumber), "The line number must be greater than 0");
        Orientation enumOrientation = (Orientation)Enum.Parse(typeof(Orientation), orientation, true);

        Task<Station> station1 = stationFinder.GetStation(nameStation1);
        Task<Station> station2 = stationFinder.GetStation(nameStation2);
        Station[] stations = await Task.WhenAll(station1, station2);

        Link linkAdd = new Link(stations[0].NameStation, stations[1].NameStation, lineNumber, enumOrientation, 5, 5);
        return await linkRegistration.AddLink(linkAdd);
    }
}