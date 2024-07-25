using api_csharp_uplink.Composant;
using api_csharp_uplink.Controllers;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Controllers;

public class LinkControllerTest
{
    private readonly LinkController _linkController;

    private readonly LinkDto _linkDtoStation125 = new()
    {
        LineNumber = 5, NameStation1 = "Station1",
        NameStation2 = "Station2", Orientation = "FORWARD"
    };

    private readonly LinkDto _linkDtoStation124 = new()
    {
        LineNumber = 4, NameStation1 = "Station1",
        NameStation2 = "Station2", Orientation = "FORWARD"
    };

    public LinkControllerTest()
    {
        IStationRepository stationRepository = new DbTestStation();
        stationRepository.Add(new Station(10.0, 10.0, "Station1"));
        stationRepository.Add(new Station(15.0, 15.0, "Station2"));
        stationRepository.Add(new Station(20.0, 20.0, "Station3"));

        StationComposant stationComposant = new StationComposant(stationRepository);
        LinkComposant linkComposant = new LinkComposant(new DbTestLink());
        TimeEngine timeEngine = new TimeEngine(linkComposant, stationComposant);

        _linkController = new LinkController(timeEngine, linkComposant);
    }

    private static void VerifyObjectResult<T>(LinkDto scheduleExpected, IActionResult result) where T : ObjectResult
    {
        result.Should().BeOfType<T>();
        T okObjectResult = (T)result;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(scheduleExpected);
    }

    private static void VerifyObjectResult<T>(List<LinkDto> schedulesExpected, IActionResult result)
        where T : ObjectResult
    {
        result.Should().BeOfType<T>();
        T okObjectResult = (T)result;
        okObjectResult.Should().NotBeNull();
        okObjectResult.Value.Should().NotBeNull();
        okObjectResult.Value.Should().BeEquivalentTo(schedulesExpected);
    }

    private static LinkDto GenerateLinkDto(string nameStation1, string nameStation2, int lineNumber)
    {
        return new LinkDto
        {
            LineNumber = lineNumber, NameStation1 = nameStation1,
            NameStation2 = nameStation2, Orientation = "FORWARD"
        };
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task AddLink()
    {
        IActionResult result = await _linkController.AddLink(_linkDtoStation125);
        VerifyObjectResult<CreatedResult>(_linkDtoStation125, result);
        result = await _linkController.FindLink(_linkDtoStation125.NameStation1, _linkDtoStation125.NameStation2,
            _linkDtoStation125.LineNumber);
        VerifyObjectResult<OkObjectResult>(_linkDtoStation125, result);

        result = await _linkController.AddLink(_linkDtoStation124);
        VerifyObjectResult<CreatedResult>(_linkDtoStation124, result);
        result = await _linkController.FindLink(_linkDtoStation124.NameStation1, _linkDtoStation124.NameStation2,
            _linkDtoStation124.LineNumber);
        VerifyObjectResult<OkObjectResult>(_linkDtoStation124, result);

        LinkDto linkExpected =
            GenerateLinkDto(_linkDtoStation125.NameStation1, "Station3", _linkDtoStation125.LineNumber);
        result = await _linkController.AddLink(linkExpected);
        VerifyObjectResult<CreatedResult>(linkExpected, result);
        result = await _linkController.FindLink(linkExpected.NameStation1, linkExpected.NameStation2,
            linkExpected.LineNumber);
        VerifyObjectResult<OkObjectResult>(linkExpected, result);
    }


    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestAddLinkError()
    {
        LinkDto linkDtoErrorLine = GenerateLinkDto(_linkDtoStation125.NameStation1, _linkDtoStation125.NameStation2, 0);
        IActionResult result = await _linkController.AddLink(linkDtoErrorLine);
        result.Should().BeOfType<BadRequestObjectResult>();

        LinkDto linkDtoErrorName = GenerateLinkDto("", _linkDtoStation125.NameStation2, _linkDtoStation125.LineNumber);
        result = await _linkController.AddLink(linkDtoErrorName);
        result.Should().BeOfType<BadRequestObjectResult>();
        linkDtoErrorName = GenerateLinkDto(_linkDtoStation125.NameStation1, "", _linkDtoStation125.LineNumber);
        result = await _linkController.AddLink(linkDtoErrorName);
        result.Should().BeOfType<BadRequestObjectResult>();

        LinkDto linkDtoUnknownStation =
            GenerateLinkDto("Station4", _linkDtoStation125.NameStation2, _linkDtoStation125.LineNumber);
        result = await _linkController.AddLink(linkDtoUnknownStation);
        result.Should().BeOfType<NotFoundObjectResult>();
        linkDtoUnknownStation =
            GenerateLinkDto(_linkDtoStation125.NameStation1, "Station4", _linkDtoStation125.LineNumber);
        result = await _linkController.AddLink(linkDtoUnknownStation);
        result.Should().BeOfType<NotFoundObjectResult>();

        await _linkController.AddLink(_linkDtoStation125);

        result = await _linkController.AddLink(_linkDtoStation125);
        result.Should().BeOfType<ConflictObjectResult>();
        LinkDto linkDtoErrorSwitchName =
            GenerateLinkDto("Station2", _linkDtoStation125.NameStation1, _linkDtoStation125.LineNumber);
        result = await _linkController.AddLink(linkDtoErrorSwitchName);
        result.Should().BeOfType<ConflictObjectResult>();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task FindLink()
    {
        await _linkController.AddLink(_linkDtoStation125);
        IActionResult result = await _linkController.FindLink(_linkDtoStation125.NameStation1,
            _linkDtoStation125.NameStation2, _linkDtoStation125.LineNumber);
        VerifyObjectResult<OkObjectResult>(_linkDtoStation125, result);

        result = await _linkController.FindLink("Station2", _linkDtoStation125.NameStation1,
            _linkDtoStation125.LineNumber);
        VerifyObjectResult<OkObjectResult>(_linkDtoStation125, result);

        result = await _linkController.FindLink(_linkDtoStation125.NameStation1, _linkDtoStation125.NameStation2,
            _linkDtoStation124.LineNumber);
        result.Should().BeOfType<NotFoundObjectResult>();
        result = await _linkController.FindLink("Station3", _linkDtoStation125.NameStation2,
            _linkDtoStation125.LineNumber);
        result.Should().BeOfType<NotFoundObjectResult>();
        result = await _linkController.FindLink(_linkDtoStation125.NameStation1, "Station3",
            _linkDtoStation125.LineNumber);
        result.Should().BeOfType<NotFoundObjectResult>();

        LinkDto linkDtoErrorLine = GenerateLinkDto(_linkDtoStation125.NameStation1, _linkDtoStation125.NameStation2, 0);
        result = await _linkController.FindLink(linkDtoErrorLine.NameStation1, linkDtoErrorLine.NameStation2,
            linkDtoErrorLine.LineNumber);
        result.Should().BeOfType<BadRequestObjectResult>();

        LinkDto linkDtoErrorName = GenerateLinkDto("", _linkDtoStation125.NameStation2, _linkDtoStation125.LineNumber);
        result = await _linkController.FindLink(linkDtoErrorName.NameStation1, linkDtoErrorName.NameStation2,
            linkDtoErrorName.LineNumber);
        result.Should().BeOfType<BadRequestObjectResult>();
        linkDtoErrorName = GenerateLinkDto(_linkDtoStation125.NameStation1, "", _linkDtoStation125.LineNumber);
        result = await _linkController.FindLink(linkDtoErrorName.NameStation1, linkDtoErrorName.NameStation2,
            linkDtoErrorName.LineNumber);
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task FindLinksByLineNumber()
    {
        await _linkController.AddLink(_linkDtoStation125);
        IActionResult result = await _linkController.FindLinksByLineNumber(_linkDtoStation125.LineNumber);
        VerifyObjectResult<OkObjectResult>([_linkDtoStation125], result);

        await _linkController.AddLink(_linkDtoStation124);
        result = await _linkController.FindLinksByLineNumber(_linkDtoStation125.LineNumber);
        VerifyObjectResult<OkObjectResult>([_linkDtoStation125], result);
        result = await _linkController.FindLinksByLineNumber(_linkDtoStation124.LineNumber);
        VerifyObjectResult<OkObjectResult>([_linkDtoStation124], result);

        LinkDto linkDto135 = GenerateLinkDto(_linkDtoStation125.NameStation1, "Station3", 5);
        await _linkController.AddLink(linkDto135);
        result = await _linkController.FindLinksByLineNumber(linkDto135.LineNumber);
        VerifyObjectResult<OkObjectResult>([_linkDtoStation125, linkDto135], result);

        result = await _linkController.FindLinksByLineNumber(1);
        VerifyObjectResult<OkObjectResult>([], result);

        result = await _linkController.FindLinksByLineNumber(0);
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task FindAllLinks()
    {
        IActionResult result = await _linkController.FindAllLinks();
        VerifyObjectResult<OkObjectResult>([], result);

        await _linkController.AddLink(_linkDtoStation125);
        result = await _linkController.FindAllLinks();
        VerifyObjectResult<OkObjectResult>([_linkDtoStation125], result);

        LinkDto linkExpected =
            GenerateLinkDto(_linkDtoStation125.NameStation1, "Station3", _linkDtoStation125.LineNumber);
        await _linkController.AddLink(_linkDtoStation124);
        await _linkController.AddLink(linkExpected);

        result = await _linkController.FindAllLinks();
        VerifyObjectResult<OkObjectResult>([_linkDtoStation125, _linkDtoStation124, linkExpected], result);
    }
}