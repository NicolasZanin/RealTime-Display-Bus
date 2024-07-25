using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface IAddLink
{
    public Task<Link> AddLink(string nameStation1, string nameStation2, int lineNumber, string orientation);
}