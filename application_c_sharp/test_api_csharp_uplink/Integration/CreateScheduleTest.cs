using System.Net;
using System.Text;
using api_csharp_uplink.Dto;
using FluentAssertions;
using Newtonsoft.Json;
using test_api_csharp_uplink.Integration.DBTest;
using Xunit.Abstractions;

namespace test_api_csharp_uplink.Integration;

[Collection("NonParallel")]
public class CreateScheduleTest(ITestOutputHelper testOutputHelper) : IAsyncLifetime
{
    private readonly HttpClient _client = new();
    private readonly string _request = "http://api_csharp_uplink:8000/api/Schedule";
    private readonly InfluxDBTest _influxDbTest = new();
    private static readonly HourDto HourDto1010 = new(){Hour = 10, Minute = 10};
    private static readonly HourDto HourDto1020 = new(){Hour = 10, Minute = 20};
    private readonly ScheduleDto _scheduleDtoStation1Forward = new(){ NameStation = "Station1", LineNumber = 1, Orientation = "FORWARD", Hours = [HourDto1010] };
    

    public async Task InitializeAsync()
    {
        await _influxDbTest.InitializeBucket();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private void VerifyResponseSuccess(HttpResponseMessage response, HttpStatusCode statusCode, string? jsonExpected)
    {
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(statusCode);

        if (jsonExpected == null) 
            return;
        
        string responseString = response.Content.ReadAsStringAsync().Result;
        responseString.Should().NotBeNullOrEmpty();
        responseString.Should().BeEquivalentTo(jsonExpected);
    } 
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task TestAddGetScheduleSimple()
    {
        try
        {
            string jsonStation1Forward = JsonConvert.SerializeObject(_scheduleDtoStation1Forward);
            StringContent content = new(jsonStation1Forward, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_request, content);
            VerifyResponseSuccess(response, HttpStatusCode.Created, null);

            response = await _client.GetAsync(_request + "/forward/Station1/1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonStation1Forward);
            
            jsonStation1Forward = JsonConvert.SerializeObject(new List<ScheduleDto>{_scheduleDtoStation1Forward});
            response = await _client.GetAsync(_request + "/forward/Station1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonStation1Forward);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task TestAddGetScheduleMultiple()
    {
        try
        {
            ScheduleDto scheduleDtoStation1Forward1020 = new(){ NameStation = "Station1", LineNumber = 1, Orientation = "FORWARD", Hours = [HourDto1010, HourDto1020] };
            string jsonStation1Forward1020 = JsonConvert.SerializeObject(scheduleDtoStation1Forward1020);
            StringContent content = new(jsonStation1Forward1020, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(_request, content);
            VerifyResponseSuccess(response, HttpStatusCode.Created, null);
            
            response = await _client.GetAsync(_request + "/forward/Station1/1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonStation1Forward1020);
            
            jsonStation1Forward1020 = JsonConvert.SerializeObject(new List<ScheduleDto>{scheduleDtoStation1Forward1020});
            response = await _client.GetAsync(_request + "/forward/Station1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonStation1Forward1020);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TestGetAddSchedule4Station()
    {
        try
        {
            ScheduleDto scheduleDtoStation1Forward1020 = new(){ NameStation = "Station1", LineNumber = 1, Orientation = "FORWARD", Hours = [HourDto1010, HourDto1020] };
            ScheduleDto scheduleDtoStation1Forward10202 = new(){ NameStation = "Station1", LineNumber = 2, Orientation = "FORWARD", Hours = [HourDto1010, HourDto1020] };
            ScheduleDto scheduleDtoStation1Backward1020 = new(){ NameStation = "Station1", LineNumber = 1, Orientation = "BACKWARD", Hours = [HourDto1010, HourDto1020] };
            ScheduleDto scheduleDtoStation2Forward1020 = new(){ NameStation = "Station2", LineNumber = 1, Orientation = "FORWARD", Hours = [HourDto1010, HourDto1020] };
            
            string jsonSchedule = JsonConvert.SerializeObject(scheduleDtoStation1Forward1020);
            StringContent content = new(jsonSchedule, Encoding.UTF8, "application/json");
            await _client.PostAsync(_request, content);
            jsonSchedule = JsonConvert.SerializeObject(scheduleDtoStation1Forward10202);
            content = new(jsonSchedule, Encoding.UTF8, "application/json");
            await _client.PostAsync(_request, content);
            jsonSchedule = JsonConvert.SerializeObject(scheduleDtoStation1Backward1020);
            content = new(jsonSchedule, Encoding.UTF8, "application/json");
            await _client.PostAsync(_request, content);
            jsonSchedule = JsonConvert.SerializeObject(scheduleDtoStation2Forward1020);
            content = new(jsonSchedule, Encoding.UTF8, "application/json");
            await _client.PostAsync(_request, content);
            
            jsonSchedule = JsonConvert.SerializeObject(new List<ScheduleDto>{scheduleDtoStation1Forward1020, scheduleDtoStation1Forward10202});
            HttpResponseMessage response = await _client.GetAsync(_request + "/forward/Station1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonSchedule);
            
            jsonSchedule = JsonConvert.SerializeObject(scheduleDtoStation1Forward1020);
            response = await _client.GetAsync(_request + "/forward/Station1/1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonSchedule);
            
            jsonSchedule = JsonConvert.SerializeObject(scheduleDtoStation1Forward10202);
            response = await _client.GetAsync(_request + "/forward/Station1/2");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonSchedule);
            
            jsonSchedule = JsonConvert.SerializeObject(scheduleDtoStation1Backward1020);
            response = await _client.GetAsync(_request + "/backward/Station1/1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonSchedule);
            
            jsonSchedule = JsonConvert.SerializeObject(new List<ScheduleDto>{scheduleDtoStation1Backward1020});
            response = await _client.GetAsync(_request + "/backward/Station1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonSchedule);
            
            jsonSchedule = JsonConvert.SerializeObject(scheduleDtoStation2Forward1020);
            response = await _client.GetAsync(_request + "/forward/Station2/1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonSchedule);
            
            jsonSchedule = JsonConvert.SerializeObject(new List<ScheduleDto>{scheduleDtoStation2Forward1020});
            response = await _client.GetAsync(_request + "/forward/Station2");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonSchedule);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task TestGetAddTechnicSchedule()
    {
        try
        {
            ScheduleDto scheduleDtoStation1Forward10101010= new(){ NameStation = "Station1", LineNumber = 1, Orientation = "FORWARD", Hours = [HourDto1010, HourDto1010] };
            string jsonScheduleStation1Forward10101010 = JsonConvert.SerializeObject(scheduleDtoStation1Forward10101010);
            StringContent contentScheduleStation1Forward10101010 = new(jsonScheduleStation1Forward10101010, Encoding.UTF8, "application/json");
            
            ScheduleDto scheduleDtoStation1Forward1010= new(){ NameStation = "Station1", LineNumber = 1, Orientation = "FORWARD", Hours = [HourDto1010] };
            string jsonScheduleStation1Forward1010 = JsonConvert.SerializeObject(scheduleDtoStation1Forward1010);
            
            HttpResponseMessage response = await _client.PostAsync(_request, contentScheduleStation1Forward10101010);
            VerifyResponseSuccess(response, HttpStatusCode.Created, jsonScheduleStation1Forward1010);
            
            response = await _client.GetAsync(_request + "/forward/Station1/1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonScheduleStation1Forward1010);
            
            ScheduleDto scheduleDtoStation1Forward10101020= new(){ NameStation = "Station1", LineNumber = 1, Orientation = "FORWARD", Hours = [HourDto1010, HourDto1020] };
            string jsonScheduleStation1Forward10101020 = JsonConvert.SerializeObject(scheduleDtoStation1Forward10101020);
            StringContent contentScheduleStation1Forward10101020 = new(jsonScheduleStation1Forward10101020, Encoding.UTF8, "application/json");
            
            ScheduleDto scheduleDtoStation1Forward1020 = new(){ NameStation = "Station1", LineNumber = 1, Orientation = "FORWARD", Hours = [HourDto1020] };
            string jsonScheduleStation1Forward1020 = JsonConvert.SerializeObject(scheduleDtoStation1Forward1020);
            
            response = await _client.PostAsync(_request, contentScheduleStation1Forward10101020);
            VerifyResponseSuccess(response, HttpStatusCode.Created, jsonScheduleStation1Forward1020);
            
            response = await _client.GetAsync(_request + "/forward/Station1/1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonScheduleStation1Forward10101020);
            
            jsonScheduleStation1Forward1010 = JsonConvert.SerializeObject(new List<ScheduleDto>{scheduleDtoStation1Forward10101020});
            response = await _client.GetAsync(_request + "/forward/Station1");
            VerifyResponseSuccess(response, HttpStatusCode.OK, jsonScheduleStation1Forward1010);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task TestAddScheduleError()
    {
        try
        {
            ScheduleDto scheduleDtoErrorName = new(){ NameStation = "", LineNumber = 1, Orientation = "FORWARD", Hours = [HourDto1010] };
            ScheduleDto scheduleDtoErrorLineNumber = new(){ NameStation = "Station1", LineNumber = 0, Orientation = "FORWARD", Hours = [HourDto1010] };
            ScheduleDto scheduleDtoErrorOrientation = new(){ NameStation = "Station1", LineNumber = 1, Orientation = "FORWAR", Hours = [HourDto1010] };
            ScheduleDto scheduleDtoErrorHours = new(){ NameStation = "Station1", LineNumber = 1, Orientation = "FORWARD", Hours = [] };
            
            string jsonScheduleErrorName = JsonConvert.SerializeObject(scheduleDtoErrorName);
            string jsonScheduleErrorLineNumber = JsonConvert.SerializeObject(scheduleDtoErrorLineNumber);
            string jsonScheduleErrorOrientation = JsonConvert.SerializeObject(scheduleDtoErrorOrientation);
            string jsonScheduleErrorHours = JsonConvert.SerializeObject(scheduleDtoErrorHours);
            
            StringContent contentScheduleErrorName = new(jsonScheduleErrorName, Encoding.UTF8, "application/json");
            StringContent contentScheduleErrorLineNumber = new(jsonScheduleErrorLineNumber, Encoding.UTF8, "application/json");
            StringContent contentScheduleErrorOrientation = new(jsonScheduleErrorOrientation, Encoding.UTF8, "application/json");
            StringContent contentScheduleErrorHours = new(jsonScheduleErrorHours, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await _client.PostAsync(_request, contentScheduleErrorName);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            response = await _client.PostAsync(_request, contentScheduleErrorLineNumber);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            response = await _client.PostAsync(_request, contentScheduleErrorOrientation);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            response = await _client.PostAsync(_request, contentScheduleErrorHours);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            
            string jsonSchedule = JsonConvert.SerializeObject(_scheduleDtoStation1Forward);
            StringContent content = new(jsonSchedule, Encoding.UTF8, "application/json");
            response = await _client.PostAsync(_request, content);
            VerifyResponseSuccess(response, HttpStatusCode.Created, null);
            
            response = await _client.PostAsync(_request, content);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        catch (HttpRequestException e)
        {
            testOutputHelper.WriteLine($"Request error: {e.Message}");
            Assert.True(false);
        }
    }
}