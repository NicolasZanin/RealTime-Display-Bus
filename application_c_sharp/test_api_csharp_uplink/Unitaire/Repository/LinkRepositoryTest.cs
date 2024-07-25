using api_csharp_uplink.DB;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Repository;
using Moq;

namespace test_api_csharp_uplink.Unitaire.Repository;

public class LinkRepositoryTest
{
    private const string MeasurementLink = "link";
    private static readonly Link LinkStation124 = new("Station1", "Station2", 4, Orientation.FORWARD, 5, 5);
    private static readonly Link LinkStation125 = new("Station1", "Station2", 5, Orientation.FORWARD, 10, 10);
    private static readonly Link LinkStation135 = new("Station1", "Station3", 5, Orientation.FORWARD, 15, 15);

    private static readonly LinkDb LinkDbStation124 = new()
    {
        LineNumber = 4, NameStation1 = "Station1", NameStation2 = "Station2", Orientation = "FORWARD", Distance = 5,
        Seconds = 5
    };

    private static readonly LinkDb LinkDbStation125 = new()
    {
        NameStation1 = "Station1", NameStation2 = "Station2", LineNumber = 5, Orientation = "FORWARD", Distance = 10,
        Seconds = 10
    };

    private static readonly LinkDb LinkDbStation135 = new()
    {
        LineNumber = 5, NameStation1 = "Station1", NameStation2 = "Station3", Orientation = "FORWARD", Distance = 15,
        Seconds = 15
    };

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestAddLink()
    {
        Mock<IGlobalInfluxDb> mock = new();
        mock.Setup(globalInfluxDb => globalInfluxDb.Save(It.IsAny<LinkDb>()))
            .ReturnsAsync(LinkDbStation125);
        LinkRepository linkRepository = new(mock.Object);

        Link result = await linkRepository.AddLink(LinkStation125);
        Assert.Equal(LinkStation125, result);
    }

    private static string GeneratePredicate(string nameStation1, string nameStation2, int lineNumber)
    {
        return $"|> filter(fn: (r) => r.lineNumber == \"{lineNumber}\" and " +
               $"((r.nameStation1 == \"{nameStation1}\" and r.nameStation2 == \"{nameStation2}\") " +
               $"or (r.nameStation1 == \"{nameStation2}\" and r.nameStation2 == \"{nameStation1}\")))";
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestGetLink()
    {
        Mock<IGlobalInfluxDb> mock = new();
        mock.SetupSequence(globalInfluxDb => globalInfluxDb.Get<LinkDb>(MeasurementLink,
                GeneratePredicate(LinkStation125.nameStation1, LinkStation125.nameStation2, LinkStation125.lineNumber)))
            .ReturnsAsync([LinkDbStation125])
            .ReturnsAsync([LinkDbStation125, LinkDbStation124]) // Supposed to return only one element
            .ReturnsAsync([]);

        LinkRepository linkRepository = new(mock.Object);
        Link? result = await linkRepository.FindLink(LinkStation125.nameStation1, LinkStation125.nameStation2,
            LinkStation125.lineNumber);
        Assert.Equal(LinkStation125, result);

        result = await linkRepository.FindLink(LinkStation125.nameStation1, LinkStation125.nameStation2,
            LinkStation125.lineNumber);
        Assert.Equal(LinkStation125, result);

        result = await linkRepository.FindLink(LinkStation125.nameStation1, LinkStation125.nameStation2,
            LinkStation125.lineNumber);
        Assert.Null(result);

        mock.Setup(globalInfluxDb => globalInfluxDb.Get<LinkDb>(MeasurementLink,
                GeneratePredicate(LinkStation124.nameStation1, LinkStation124.nameStation2, LinkStation124.lineNumber)))
            .ReturnsAsync([LinkDbStation124]);

        result = await linkRepository.FindLink(LinkStation124.nameStation1, LinkStation124.nameStation2,
            LinkStation124.lineNumber);
        Assert.Equal(LinkStation124, result);

        mock.Setup(globalInfluxDb => globalInfluxDb.Get<LinkDb>(MeasurementLink,
                GeneratePredicate(LinkStation135.nameStation1, LinkStation135.nameStation2, LinkStation135.lineNumber)))
            .ReturnsAsync([LinkDbStation135]);
        result = await linkRepository.FindLink(LinkStation135.nameStation1, LinkStation135.nameStation2,
            LinkStation135.lineNumber);
        Assert.Equal(LinkStation135, result);

        Link linkExpected = new Link("Station3", "Station1", 5, Orientation.FORWARD, 15, 15);
        LinkDb linkDb = new()
        {
            LineNumber = 5, NameStation1 = "Station3", NameStation2 = "Station1", Orientation = "FORWARD",
            Distance = 15, Seconds = 15
        };

        mock.Setup(globalInfluxDb => globalInfluxDb.Get<LinkDb>(MeasurementLink,
                GeneratePredicate(linkExpected.nameStation1, linkExpected.nameStation2, linkExpected.lineNumber)))
            .ReturnsAsync([linkDb]);
        result = await linkRepository.FindLink(linkExpected.nameStation1, linkExpected.nameStation2,
            linkExpected.lineNumber);
        Assert.Equal(linkExpected, result);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestGetLinkByLineNumber()
    {
        Mock<IGlobalInfluxDb> mock = new();
        mock.Setup(globalInfluxDb => globalInfluxDb.Get<LinkDb>(MeasurementLink,
                $"|> filter(fn: (r) => r.lineNumber == \"{LinkDbStation125.LineNumber}\")"))
            .ReturnsAsync([LinkDbStation125, LinkDbStation135]);
        LinkRepository linkRepository = new(mock.Object);

        List<Link> result = await linkRepository.FindLinksByLineNumber(LinkStation125.lineNumber);
        Assert.Equal([LinkStation125, LinkStation135], result);

        mock.Setup(globalInfluxDb => globalInfluxDb.Get<LinkDb>(MeasurementLink,
                $"|> filter(fn: (r) => r.lineNumber == \"1\")"))
            .ReturnsAsync([]);

        result = await linkRepository.FindLinksByLineNumber(1);
        Assert.Empty(result);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestGetAllLinks()
    {
        Mock<IGlobalInfluxDb> mock = new();
        mock.Setup(globalInfluxDb => globalInfluxDb.GetAll<LinkDb>(MeasurementLink))
            .ReturnsAsync([LinkDbStation124, LinkDbStation125, LinkDbStation135]);
        LinkRepository linkRepository = new(mock.Object);

        List<Link> result = await linkRepository.FindAllLinks();
        Assert.Equal([LinkStation124, LinkStation125, LinkStation135], result);

        mock.Setup(globalInfluxDb => globalInfluxDb.GetAll<LinkDb>(MeasurementLink))
            .ReturnsAsync([]);

        result = await linkRepository.FindAllLinks();
        Assert.Empty(result);
    }
}