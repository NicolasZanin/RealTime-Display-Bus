using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Repository;

public class LinkRepository(IGlobalInfluxDb globalInfluxDb) : ILinkRepository
{
    private const string MeasurementLink = "link";

    public async Task<Link> AddLink(Link link)
    {
        LinkDb linkDb = await globalInfluxDb.Save(ConvertToLinkDb(link));
        return ConvertToLink(linkDb);
    }

    public async Task<Link?> FindLink(string nameStation1, string nameStation2, int lineNumber)
    {
        string predicate = $"|> filter(fn: (r) => r.lineNumber == \"{lineNumber}\" and " +
                           $"((r.nameStation1 == \"{nameStation1}\" and r.nameStation2 == \"{nameStation2}\") " +
                           $"or (r.nameStation1 == \"{nameStation2}\" and r.nameStation2 == \"{nameStation1}\")))";

        List<LinkDb> links = await globalInfluxDb.Get<LinkDb>(MeasurementLink, predicate);
        return links.Count == 0 ? null : ConvertToLink(links[0]);
    }

    public async Task<List<Link>> FindLinksByLineNumber(int lineNumber)
    {
        string predicate = $"|> filter(fn: (r) => r.lineNumber == \"{lineNumber}\")";
        List<LinkDb> links = await globalInfluxDb.Get<LinkDb>(MeasurementLink, predicate);

        return links.Select(ConvertToLink).ToList();
    }

    public async Task<List<Link>> FindAllLinks()
    {
        List<LinkDb> links = await globalInfluxDb.GetAll<LinkDb>(MeasurementLink);
        return links.Select(ConvertToLink).ToList();
    }

    private static LinkDb ConvertToLinkDb(Link link)
    {
        return new LinkDb
        {
            NameStation1 = link.nameStation1,
            NameStation2 = link.nameStation2,
            LineNumber = link.lineNumber,
            Orientation = link.orientation.ToString(),
            Distance = link.distance,
            Seconds = link.seconds
        };
    }

    private static Link ConvertToLink(LinkDb linkDb)
    {
        Orientation enumOrientation = (Orientation)Enum.Parse(typeof(Orientation), linkDb.Orientation, true);

        return new Link(linkDb.NameStation1, linkDb.NameStation2, linkDb.LineNumber, enumOrientation,
            linkDb.Distance, linkDb.Seconds);
    }
}