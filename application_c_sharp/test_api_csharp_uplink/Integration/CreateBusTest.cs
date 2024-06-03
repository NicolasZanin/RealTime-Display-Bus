using api_csharp_uplink.Dto;
using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace test_api_csharp_uplink.Integration
{

    public class CreateBusTest
    {
        private readonly HttpClient _client;
        private readonly string _request = "http://api_csharp_uplink:8000/api/bus";

        public CreateBusTest()
        {
            _client = new();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestGetBuses()
        {
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
                Console.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestAddBusNormal()
        {
            BusDTO bus = new()
            {
                LineBus = 1,
                BusNumber = 0,
                DevEUICard = 0
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
                Console.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestAddBusError()
        {
            BusDTO bus = new()
            {
                LineBus = 1,
                BusNumber = 1,
                DevEUICard = 1
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
                Console.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestGetBus()
        {
            BusDTO bus = new()
            {
                LineBus = 2,
                BusNumber = 2,
                DevEUICard = 2
            };

            string json = JsonConvert.SerializeObject(bus);
            StringContent content = new(JsonConvert.SerializeObject(bus), Encoding.UTF8, "application/json");

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
                response = await _client.GetAsync($"{_request}/DevEUI/{bus.DevEUICard}");
                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);

                responseString = await response.Content.ReadAsStringAsync();
                responseString.Should().NotBeNullOrEmpty();
                responseString.Should().BeEquivalentTo(json);

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task TestGetAllBus()
        {
            BusDTO bus = new()
            {
                LineBus = 2,
                BusNumber = 2,
                DevEUICard = 2
            };

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_request);

                response.EnsureSuccessStatusCode();
                response.StatusCode.Should().Be(HttpStatusCode.OK);
                string responseString = await response.Content.ReadAsStringAsync();
                responseString.Should().NotBeNullOrEmpty();

                List<BusDTO>? buses = JsonConvert.DeserializeObject<List<BusDTO>>(responseString);
                buses.Should().NotBeNull();
                buses.Should().HaveCount(3);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                Assert.True(false);
            }
        }
    }
}
