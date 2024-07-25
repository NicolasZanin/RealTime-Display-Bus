using api_csharp_uplink.Entities;

namespace api_csharp_uplink.Interface;

public interface ILinkRegistration
{
    public Task<Link> AddLink(Link link);
}