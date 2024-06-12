using api_csharp_uplink.Dto;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using test_api_csharp_uplink.Integration.DBTest;
using Xunit.Abstractions;

namespace test_api_csharp_uplink.Integration
{

    [Collection("NonParallel")]
    public class CreateBusTest(ITestOutputHelper testOutputHelper) : IAsyncLifetime
    {
        private readonly HttpClient _client = new();
        private readonly string _request = "http://api_csharp_uplink:8000/api/bus";
        private readonly InfluxDBTest _influxDbTest = new();

        public async Task InitializeAsync()
        {
            await _influxDbTest.InitializeBucket();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestGetBuses()
        {
            await _influxDbTest.InitializeBucket();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_request);

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string responseString = await response.Content.ReadAsStringAsync();
                responseString.Should().NotBeNullOrEmpty();
                responseString.Should().Be("[]");
            }
            catch (HttpRequestException e)
            {
                testOutputHelper.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestAddBusNormal()
        {
            BusDto bus = new()
            {
                LineBus = 1,
                BusNumber = 0,
                DevEuiCard = "0"
            };

            string json = JsonConvert.SerializeObject(bus);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _client.PostAsync(_request, content);
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.Created);

                string responseString = await response.Content.ReadAsStringAsync();
                responseString.Should().NotBeNullOrEmpty();
                responseString.Should().BeEquivalentTo(json);
            }
            catch (HttpRequestException e)
            {
                testOutputHelper.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestAddBusError()
        {
            BusDto bus = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEuiCard = "1"
            };

            StringContent content = new(JsonConvert.SerializeObject(bus), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _client.PostAsync(_request, content);
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.Created);

                response = await _client.PostAsync(_request, content);
                response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            }
            catch (HttpRequestException e)
            {
                testOutputHelper.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestGetBus()
        {
            BusDto bus = new()
            {
                LineBus = 2,
                BusNumber = 2,
                DevEuiCard = "2"
            };

            string json = JsonConvert.SerializeObject(bus);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            try
            {
                // Add bus
                HttpResponseMessage response = await _client.PostAsync(_request, content);
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.Created);
                // Get bus by bus number
                response = await _client.GetAsync($"{_request}/busNumber/{bus.BusNumber}");
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string responseString = await response.Content.ReadAsStringAsync();
                responseString.Should().NotBeNullOrEmpty();
                responseString.Should().BeEquivalentTo(json);

                // Get bus by DevEUI
                response = await _client.GetAsync($"{_request}/devEuiCard/{bus.DevEuiCard}");
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                responseString = await response.Content.ReadAsStringAsync();
                responseString.Should().NotBeNullOrEmpty();
                responseString.Should().BeEquivalentTo(json);

            }
            catch (HttpRequestException e)
            {
                testOutputHelper.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestGetAllBus()
        {
            BusDto bus = new()
            {
                LineBus = 2,
                BusNumber = 0,
                DevEuiCard = "0"
            };
            try
            {
                await _client.PostAsync(_request, new StringContent(JsonConvert.SerializeObject(bus), Encoding.UTF8, "application/json"));

                bus.BusNumber = 1;
                await _client.PostAsync(_request, new StringContent(JsonConvert.SerializeObject(bus), Encoding.UTF8, "application/json"));

                bus.BusNumber = 2;
                await _client.PostAsync(_request, new StringContent(JsonConvert.SerializeObject(bus), Encoding.UTF8, "application/json"));

                HttpResponseMessage response = await _client.GetAsync(_request);
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string responseString = await response.Content.ReadAsStringAsync();
                responseString.Should().NotBeNullOrEmpty();

                List<BusDto>? buses = JsonConvert.DeserializeObject<List<BusDto>>(responseString);
                buses.Should().NotBeNull();
                buses.Should().HaveCount(3);
            }
            catch (HttpRequestException e)
            {
                testOutputHelper.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }
    }
}
