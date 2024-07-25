using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface ILinkRepository
{
    public Task<Link> AddLink(Link link);
    public Task<Link?> FindLink(string nameStation1, string nameStation2, int lineNumber);
    public Task<List<Link>> FindLinksByLineNumber(int lineNumber);
    public Task<List<Link>> FindAllLinks();
}