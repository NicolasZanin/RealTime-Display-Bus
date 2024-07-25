using System.Net;
using System.Text;
using api_csharp_uplink.Dto;
using FluentAssertions;
using Newtonsoft.Json;
using test_api_csharp_uplink.Integration.DBTest;
using Xunit.Abstractions;

namespace test_api_csharp_uplink.Integration;

[Collection("NonParallel")]
public class CreateLinkTest(ITestOutputHelper testOutputHelper) : IAsyncLifetime
{
    private readonly HttpClient _client = new();
    private const string Request = "http://api_csharp_uplink:8000/api/Link";
    private readonly InfluxDbTest _influxDbTest = new();

    private static readonly LinkDto LinkDto125 = new()
        { NameStation1 = "Station1", NameStation2 = "Station2", LineNumber = 5, Orientation = "FORWARD" };

    private static readonly LinkDto LinkDto124 = new()
        { NameStation1 = "Station1", NameStation2 = "Station2", LineNumber = 4, Orientation = "FORWARD" };

    private static readonly LinkDto LinkDto135 = new()
        { NameStation1 = "Station1", NameStation2 = "Station3", LineNumber = 5, Orientation = "FORWARD" };

    private static readonly string JsonExpected125 = JsonConvert.SerializeObject(LinkDto125);
    private static readonly string JsonExpected124 = JsonConvert.SerializeObject(LinkDto124);
    private static readonly string JsonExpected135 = JsonConvert.SerializeObject(LinkDto135);
    private static readonly StringContent Content125 = new(JsonExpected125, Encoding.UTF8, "application/json");
    private static readonly StringContent Content124 = new(JsonExpected124, Encoding.UTF8, "application/json");
    private static readonly StringContent Content135 = new(JsonExpected135, Encoding.UTF8, "application/json");

    private static StringContent CreateContent(StationDto stationDto)
    {
        string jsonStation = JsonConvert.SerializeObject(stationDto);
        return new(jsonStation, Encoding.UTF8, "application/json");
    }

    public async Task InitializeAsync()
    {
        await _influxDbTest.InitializeBucket();

        StringContent content = CreateContent(new StationDto
            { NameStation = "Station1", Position = new PositionDto { Latitude = 10.01, Longitude = 9.01 } });
        await _client.PostAsync("http://api_csharp_uplink:8000/api/Station", content);

        content = CreateContent(new StationDto
            { NameStation = "Station2", Position = new PositionDto { Latitude = 15.01, Longitude = 14.01 } });
        await _client.PostAsync("http://api_csharp_uplink:8000/api/Station", content);

        content = CreateContent(new StationDto
            { NameStation = "Station3", Position = new PositionDto { Latitude = 20.01, Longitude = 19.01 } });
        await _client.PostAsync("http://api_csharp_uplink:8000/api/Station", content);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private static async Task VerifyResponseSuccess(HttpResponseMessage response, HttpStatusCode statusCode,
        string? jsonExpected)
    {
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(statusCode);

        if (jsonExpected == null)
            return;

        string responseString = await response.Content.ReadAsStringAsync();
        responseString.Should().NotBeNullOrEmpty();
        responseString.Should().BeEquivalentTo(jsonExpected);
    }

    private static LinkDto GenerateLinkDto(string nameStation1, string nameStation2, int lineNumber)
    {
        return new LinkDto
        {
            LineNumber = lineNumber, NameStation1 = nameStation1,
            NameStation2 = nameStation2, Orientation = "FORWARD"
        };
    }

    private async Task VerifyBadRequest(string nameStation1, string nameStation2, int lineNumber,
        HttpStatusCode statusCode)
    {
        LinkDto linkDtoErrorLine = GenerateLinkDto(nameStation1, nameStation2, lineNumber);
        string jsonExpected = JsonConvert.SerializeObject(linkDtoErrorLine);
        StringContent content = new(jsonExpected, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PostAsync(Request, content);
        response.StatusCode.Should().Be(statusCode);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task AddLink()
    {
        try
        {
            HttpResponseMessage response = await _client.PostAsync(Request, Content125);
            await VerifyResponseSuccess(response, HttpStatusCode.Created, JsonExpected125);
            response = await _client.GetAsync(Request +
                                              $"/{LinkDto125.NameStation1}/{LinkDto125.NameStation2}/{LinkDto125.LineNumber}");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, JsonExpected125);

            response = await _client.PostAsync(Request, Content124);
            await VerifyResponseSuccess(response, HttpStatusCode.Created, JsonExpected124);
            response = await _client.GetAsync(Request +
                                              $"/{LinkDto124.NameStation1}/{LinkDto124.NameStation2}/{LinkDto124.LineNumber}");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, JsonExpected124);

            response = await _client.PostAsync(Request, Content135);
            await VerifyResponseSuccess(response, HttpStatusCode.Created, JsonExpected135);
            response = await _client.GetAsync(Request +
                                              $"/{LinkDto135.NameStation1}/{LinkDto135.NameStation2}/{LinkDto135.LineNumber}");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, JsonExpected135);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task AddLinkError()
    {
        try
        {
            await VerifyBadRequest(LinkDto125.NameStation1, LinkDto125.NameStation2, 0, HttpStatusCode.BadRequest);

            await VerifyBadRequest("", LinkDto125.NameStation2, LinkDto125.LineNumber, HttpStatusCode.BadRequest);

            await VerifyBadRequest(LinkDto125.NameStation1, "", LinkDto125.LineNumber, HttpStatusCode.BadRequest);

            await VerifyBadRequest("Station4", LinkDto125.NameStation2, LinkDto125.LineNumber, HttpStatusCode.NotFound);

            await VerifyBadRequest(LinkDto125.NameStation1, "Station4", LinkDto125.LineNumber, HttpStatusCode.NotFound);

            await _client.PostAsync(Request, Content125);

            await VerifyBadRequest(LinkDto125.NameStation1, LinkDto125.NameStation2, LinkDto125.LineNumber,
                HttpStatusCode.Conflict);

            await VerifyBadRequest(LinkDto125.NameStation2, LinkDto125.NameStation1, LinkDto125.LineNumber,
                HttpStatusCode.Conflict);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TestFindLink()
    {
        try
        {
            await _client.PostAsync(Request, Content125);
            HttpResponseMessage response = await _client.GetAsync(Request +
                                                                  $"/{LinkDto125.NameStation1}/{LinkDto125.NameStation2}/{LinkDto125.LineNumber}");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, JsonExpected125);

            response = await _client.GetAsync(Request +
                                              $"/{LinkDto125.NameStation2}/{LinkDto125.NameStation1}/{LinkDto125.LineNumber}");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, JsonExpected125);

            response = await _client.GetAsync(Request +
                                              $"/{LinkDto125.NameStation1}/{LinkDto125.NameStation2}/{LinkDto124.LineNumber}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            response = await _client.GetAsync(Request +
                                              $"/{LinkDto125.NameStation1}/{LinkDto135.NameStation2}/{LinkDto125.LineNumber}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            response = await _client.GetAsync(Request +
                                              $"/{LinkDto135.NameStation2}/{LinkDto125.NameStation2}/{LinkDto125.LineNumber}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            await VerifyBadRequest(LinkDto125.NameStation1, LinkDto125.NameStation2, 0, HttpStatusCode.BadRequest);

            await VerifyBadRequest("", LinkDto125.NameStation2, LinkDto125.LineNumber, HttpStatusCode.BadRequest);

            await VerifyBadRequest(LinkDto125.NameStation1, "", LinkDto125.LineNumber, HttpStatusCode.BadRequest);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TestFindLinksByLineNumber()
    {
        try
        {
            string jsonExpectedList = JsonConvert.SerializeObject(new List<LinkDto> { LinkDto125 });
            await _client.PostAsync(Request, Content125);
            HttpResponseMessage response = await _client.GetAsync(Request + $"/{LinkDto125.LineNumber}");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, jsonExpectedList);

            await _client.PostAsync(Request, Content124);
            response = await _client.GetAsync(Request + $"/{LinkDto125.LineNumber}");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, jsonExpectedList);

            jsonExpectedList = JsonConvert.SerializeObject(new List<LinkDto> { LinkDto124 });
            response = await _client.GetAsync(Request + $"/{LinkDto124.LineNumber}");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, jsonExpectedList);

            await _client.PostAsync(Request, Content135);

            jsonExpectedList = JsonConvert.SerializeObject(new List<LinkDto> { LinkDto125, LinkDto135 });
            response = await _client.GetAsync(Request + $"/{LinkDto135.LineNumber}");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, jsonExpectedList);

            response = await _client.GetAsync(Request + "/1");
            await VerifyResponseSuccess(response, HttpStatusCode.OK, "[]");

            response = await _client.GetAsync(Request + "/0");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task FindAllLinks()
    {
        try
        {
            HttpResponseMessage response = await _client.GetAsync(Request);
            await VerifyResponseSuccess(response, HttpStatusCode.OK, "[]");

            string jsonExpectedList = JsonConvert.SerializeObject(new List<LinkDto> { LinkDto125 });
            await _client.PostAsync(Request, Content125);
            response = await _client.GetAsync(Request);
            await VerifyResponseSuccess(response, HttpStatusCode.OK, jsonExpectedList);

            await _client.PostAsync(Request, Content124);
            await _client.PostAsync(Request, Content135);

            jsonExpectedList = JsonConvert.SerializeObject(new List<LinkDto> { LinkDto124, LinkDto125, LinkDto135 });
            response = await _client.GetAsync(Request);
            await VerifyResponseSuccess(response, HttpStatusCode.OK, jsonExpectedList);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }
}