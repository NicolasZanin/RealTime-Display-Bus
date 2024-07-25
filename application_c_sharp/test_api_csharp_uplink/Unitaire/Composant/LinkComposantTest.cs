using api_csharp_uplink.Composant;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Composant;

public class LinkComposantTest
{
    private readonly LinkComposant _linkComposant;

    private readonly Link _linkStation125 = new("Station1", "Station2", 5,
        Orientation.FORWARD, 25, 25);

    private readonly Link _linkStation124 = new("Station1", "Station2", 4,
        Orientation.FORWARD, 25, 25);

    public LinkComposantTest()
    {
        ILinkRepository linkRepository = new DbTestLink();
        _linkComposant = new LinkComposant(linkRepository);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task AddLink()
    {
        Link linkAdded = await _linkComposant.AddLink(_linkStation125);
        Assert.Equal(_linkStation125, linkAdded);

        Link linkGet = await _linkComposant.FindLink(_linkStation125.nameStation1, _linkStation125.nameStation2,
            _linkStation125.lineNumber);
        Assert.Equal(_linkStation125, linkGet);

        linkAdded = await _linkComposant.AddLink(_linkStation124);
        Assert.Equal(_linkStation124, linkAdded);
        linkGet = await _linkComposant.FindLink(_linkStation124.nameStation1, _linkStation124.nameStation2,
            _linkStation124.lineNumber);
        Assert.Equal(_linkStation124, linkGet);

        Link linkExpected = new Link(_linkStation125.nameStation1, "Station3", _linkStation125.lineNumber,
            Orientation.FORWARD, 25, 25);
        linkAdded = await _linkComposant.AddLink(linkExpected);
        Assert.Equal(linkExpected, linkAdded);

        linkGet = await _linkComposant.FindLink(linkExpected.nameStation1, linkExpected.nameStation2,
            linkExpected.lineNumber);
        Assert.Equal(linkExpected, linkGet);

        await Assert.ThrowsAsync<AlreadyCreateException>(() => _linkComposant.AddLink(_linkStation125));

        Link linkErrorSwitchName = new Link("Station2", linkExpected.nameStation1, linkExpected.lineNumber,
            Orientation.FORWARD, 25, 25);
        await Assert.ThrowsAsync<AlreadyCreateException>(() => _linkComposant.AddLink(linkErrorSwitchName));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task FindLink()
    {
        await _linkComposant.AddLink(_linkStation125);
        Link linkGet = await _linkComposant.FindLink(_linkStation125.nameStation1, _linkStation125.nameStation2,
            _linkStation125.lineNumber);
        Assert.Equal(_linkStation125, linkGet);

        linkGet = await _linkComposant.FindLink("Station2", _linkStation125.nameStation1, _linkStation125.lineNumber);
        Assert.Equal(_linkStation125, linkGet);

        await Assert.ThrowsAsync<NotFoundException>(() => _linkComposant.FindLink(_linkStation125.nameStation1,
            _linkStation125.nameStation2, _linkStation124.lineNumber));
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _linkComposant.FindLink("Station3", _linkStation125.nameStation2, _linkStation125.lineNumber));
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _linkComposant.FindLink(_linkStation125.nameStation1, "Station3", _linkStation125.lineNumber));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            _linkComposant.FindLink(_linkStation125.nameStation1, _linkStation125.nameStation2, 0));
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _linkComposant.FindLink("", _linkStation125.nameStation2, _linkStation125.lineNumber));
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _linkComposant.FindLink(_linkStation125.nameStation1, "", _linkStation125.lineNumber));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task FindLinksByLineNumber()
    {
        await _linkComposant.AddLink(_linkStation125);
        List<Link> links = await _linkComposant.FindLinksByLineNumber(_linkStation125.lineNumber);
        Assert.Equal(_linkStation125, Assert.Single(links));

        await _linkComposant.AddLink(_linkStation124);
        links = await _linkComposant.FindLinksByLineNumber(_linkStation125.lineNumber);
        Assert.Equal(_linkStation125, Assert.Single(links));
        links = await _linkComposant.FindLinksByLineNumber(_linkStation124.lineNumber);
        Assert.Equal(_linkStation124, Assert.Single(links));

        Link linkStation135 = new Link("Station1", "Station3", 5, Orientation.FORWARD, 25, 25);
        await _linkComposant.AddLink(linkStation135);
        links = await _linkComposant.FindLinksByLineNumber(5);
        Assert.Equal(2, links.Count);
        Assert.Equal([_linkStation125, linkStation135], links);

        links = await _linkComposant.FindLinksByLineNumber(1);
        Assert.Empty(links);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _linkComposant.FindLinksByLineNumber(0));
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task FindAllLinks()
    {
        List<Link> links = await _linkComposant.FindAllLinks();
        Assert.Empty(links);

        await _linkComposant.AddLink(_linkStation125);
        links = await _linkComposant.FindAllLinks();
        Assert.Equal(_linkStation125, Assert.Single(links));

        Link linkExpected = new Link(_linkStation125.nameStation1, "Station3", _linkStation125.lineNumber,
            Orientation.FORWARD, 25, 25);
        await _linkComposant.AddLink(_linkStation124);
        await _linkComposant.AddLink(linkExpected);

        links = await _linkComposant.FindAllLinks();
        Assert.Equal(3, links.Count);
        Assert.Equal([_linkStation125, _linkStation124, linkExpected], links);
    }
}