using api_csharp_uplink.Composant;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Composant;

public class TimeEngineTest
{
    private readonly IAddLink _addLink;
    private readonly ILinkFinder _linkFinder;

    private readonly Link _linkStation125 = new("Station1", "Station2", 5,
        Orientation.FORWARD, 25, 25);

    private readonly Link _linkStation124 = new("Station1", "Station2", 4,
        Orientation.FORWARD, 25, 25);

    public TimeEngineTest()
    {
        ILinkRepository linkRepository = new DbTestLink();
        IStationRepository stationRepository = new DbTestStation();
        LinkComposant linkRegistration = new LinkComposant(linkRepository);
        IStationFinder stationFinder = new StationComposant(stationRepository);

        stationRepository.Add(new Station(10.0, 10.0, "Station1"));
        stationRepository.Add(new Station(15.0, 15.0, "Station2"));
        stationRepository.Add(new Station(20.0, 20.0, "Station3"));

        _linkFinder = linkRegistration;
        _addLink = new TimeEngine(linkRegistration, stationFinder);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestAddLink()
    {
        Link linkAdded = await _addLink.AddLink(_linkStation125.nameStation1, _linkStation125.nameStation2,
            _linkStation125.lineNumber, _linkStation125.orientation.ToString());
        Assert.Equal(_linkStation125, linkAdded);

        Link linkGet = await _linkFinder.FindLink(_linkStation125.nameStation1, _linkStation125.nameStation2,
            _linkStation125.lineNumber);
        Assert.Equal(_linkStation125, linkGet);

        linkAdded = await _addLink.AddLink(_linkStation124.nameStation1, _linkStation124.nameStation2,
            _linkStation124.lineNumber, _linkStation124.orientation.ToString());
        Assert.Equal(_linkStation124, linkAdded);
        linkGet = await _linkFinder.FindLink(_linkStation124.nameStation1, _linkStation124.nameStation2,
            _linkStation124.lineNumber);
        Assert.Equal(_linkStation124, linkGet);

        Link linkExpected = new Link(_linkStation125.nameStation1, "Station3", _linkStation125.lineNumber,
            Orientation.FORWARD, 25, 25);
        linkAdded = await _addLink.AddLink(linkExpected.nameStation1, linkExpected.nameStation2,
            linkExpected.lineNumber, linkExpected.orientation.ToString());
        Assert.Equal(linkExpected, linkAdded);

        linkGet = await _linkFinder.FindLink(linkExpected.nameStation1, linkExpected.nameStation2,
            linkExpected.lineNumber);
        Assert.Equal(linkExpected, linkGet);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestAddLinkError()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _addLink.AddLink(_linkStation125.nameStation1,
            _linkStation125.nameStation2, 0, _linkStation125.orientation.ToString()));
        await Assert.ThrowsAsync<ArgumentNullException>(() => _addLink.AddLink("", _linkStation125.nameStation2,
            _linkStation125.lineNumber, _linkStation125.orientation.ToString()));
        await Assert.ThrowsAsync<ArgumentNullException>(() => _addLink.AddLink(_linkStation125.nameStation1, "",
            _linkStation125.lineNumber, _linkStation125.orientation.ToString()));


        await Assert.ThrowsAsync<NotFoundException>(() => _addLink.AddLink("Station4", _linkStation125.nameStation2,
            _linkStation125.lineNumber, _linkStation125.orientation.ToString()));
        await Assert.ThrowsAsync<NotFoundException>(() => _addLink.AddLink(_linkStation125.nameStation1, "Station4",
            _linkStation125.lineNumber, _linkStation125.orientation.ToString()));

        await _addLink.AddLink(_linkStation125.nameStation1, _linkStation125.nameStation2,
            _linkStation125.lineNumber, _linkStation125.orientation.ToString());

        await Assert.ThrowsAsync<AlreadyCreateException>(() => _addLink.AddLink(_linkStation125.nameStation1,
            _linkStation125.nameStation2,
            _linkStation125.lineNumber, _linkStation125.orientation.ToString()));
        await Assert.ThrowsAsync<AlreadyCreateException>(() => _addLink.AddLink("Station2",
            _linkStation125.nameStation1,
            _linkStation125.lineNumber, _linkStation125.orientation.ToString()));
    }
}