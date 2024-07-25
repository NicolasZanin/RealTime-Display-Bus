using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Composant;

public class LinkComposant(ILinkRepository linkRepository) : ILinkFinder, ILinkRegistration
{
    public async Task<Link> AddLink(Link link)
    {
        Task<Link?> findLink = linkRepository.FindLink(link.nameStation1, link.nameStation2, link.lineNumber);

        if (await findLink != null)
            throw new AlreadyCreateException(
                $"The link with NameStation1 {link.nameStation1}, NameStation2 {link.nameStation2}, LineNumber {link.lineNumber} exists");

        return await linkRepository.AddLink(link);
    }

    public async Task<Link> FindLink(string nameStation1, string nameStation2, int lineNumber)
    {
        if (lineNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(lineNumber), "The line number must be greater than 0");
        if (string.IsNullOrEmpty(nameStation1))
            throw new ArgumentNullException(nameof(nameStation1), "The name of the station must not be null or empty");
        if (string.IsNullOrEmpty(nameStation2))
            throw new ArgumentNullException(nameof(nameStation2), "The name of the station must not be null or empty");

        return await linkRepository.FindLink(nameStation1, nameStation2, lineNumber) ??
               throw new NotFoundException(
                   $"The link with NameStation1 {nameStation1}, NameStation2 {nameStation2}, LineNumber {lineNumber}");
    }

    public Task<List<Link>> FindLinksByLineNumber(int lineNumber)
    {
        if (lineNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(lineNumber), "The line number must be greater than 0");

        return linkRepository.FindLinksByLineNumber(lineNumber);
    }

    public Task<List<Link>> FindAllLinks()
    {
        return linkRepository.FindAllLinks();
    }
}