using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace test_api_csharp_uplink.Unitaire.DBTest;

public class DbTestLink : ILinkRepository
{
    private readonly List<Link> _links = [];

    public Task<Link> AddLink(Link link)
    {
        _links.Add(link);
        return Task.FromResult(link);
    }

    public Task<Link?> FindLink(string nameStation1, string nameStation2, int lineNumber)
    {
        Link linkToFind = new Link(nameStation1, nameStation2, lineNumber, Orientation.FORWARD, 1000, 10000);
        Link? link = _links.Find(l => l.Equals(linkToFind));
        return Task.FromResult(link);
    }

    public Task<List<Link>> FindLinksByLineNumber(int lineNumber)
    {
        List<Link> links = _links.Where(l => l.lineNumber == lineNumber).ToList();
        return Task.FromResult(links);
    }

    public Task<List<Link>> FindAllLinks()
    {
        return Task.FromResult(_links);
    }
}