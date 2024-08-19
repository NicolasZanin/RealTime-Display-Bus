using api_csharp_uplink.Composant;
using api_csharp_uplink.Controllers;
using api_csharp_uplink.Dto;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using api_csharp_uplink.Repository.Interface;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using test_api_csharp_uplink.Unitaire.Composant;
using test_api_csharp_uplink.Unitaire.DBTest;

namespace test_api_csharp_uplink.Unitaire.Controllers;

public class TimeEngineControllerTest : IAsyncLifetime
{
    private TimeEngineController? _timeEngineController;
    private GraphComposant? _graphComposant;
    
    public async Task InitializeAsync()
    {
        IStationRepository stationRepository = new DbTestStation();
        StationComposant stationComposant = new StationComposant(stationRepository);
        
        List<Station> stations = GenerateConnexion.AddStations();

        await Parallel.ForEachAsync(stations, async (station, _) =>
        {
            await stationComposant.AddStation(station.Position.Latitude, station.Position.Longitude,
                station.NameStation);
        });
        
        var (stationTimeDistanceForward, stationTimeDistanceBackward) = GenerateConnexion.GetStationTimeDistance(stations);

        List<Connexion> connexionsForward = GenerateConnexion.AddConnexions(stationTimeDistanceForward, "FORWARD");
        List<Connexion> connexionsBackward = GenerateConnexion.AddConnexions(stationTimeDistanceBackward, "BACKWARD");
        
        List<ConnexionDto> connexionDtosForward = GenerateConnexion.ConvertConnexionToDto(connexionsForward);
        List<ConnexionDto> connexionDtosBackward = GenerateConnexion.ConvertConnexionToDto(connexionsBackward);
        
        IItineraryRepository itineraryRepository = new DbTestItinerary();
        _graphComposant = new GraphComposant();
        GraphHopperTest graphHopperTest = new GraphHopperTest();
        IItineraryRegister itineraryRegister = new ItineraryComposant(itineraryRepository, stationComposant, graphHopperTest, _graphComposant);
        CardComposant cardComposant = new CardComposant(new DbTestCard());
        ITimeProcessor timeProcessor = new TimeComposant(_graphComposant, _graphComposant, cardComposant);
        _timeEngineController = new TimeEngineController(graphHopperTest, timeProcessor);
        
        await itineraryRegister.AddItinerary(1, "FORWARD", connexionDtosForward);
        await itineraryRegister.AddItinerary(1, "BACKWARD", connexionDtosBackward);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private static void VerifySuccessResultTime(int timeExpect, IActionResult result)
    {
        result.Should().BeOfType<OkObjectResult>();
        OkObjectResult? okObjectResult = result as OkObjectResult;
        Assert.NotNull(okObjectResult);
        Assert.Equal(200, okObjectResult.StatusCode);
        okObjectResult.Value.Should().BeOfType<int>();
        Assert.NotNull(okObjectResult.Value);
        int time = (int) okObjectResult.Value;
        Assert.Equal(timeExpect, time);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    public async Task GetTimeItinerary()
    {
        if (_timeEngineController == null)
            Assert.False(true);
        
        IActionResult result = await _timeEngineController.GetTimeDistance(1, "Station1", "Station5");
        VerifySuccessResultTime(20, result);
        
        result = await _timeEngineController.GetTimeDistance(1, "Station5", "Station1");
        VerifySuccessResultTime(20, result);
        
        result = await _timeEngineController.GetTimeDistance(1, "StationF2", "StationF4");
        VerifySuccessResultTime(10, result);
        
        result = await _timeEngineController.GetTimeDistance(1, "StationF4", "StationF2");
        VerifySuccessResultTime(10, result);
        
        result = await _timeEngineController.GetTimeDistance(1, "Station1", "StationB2");
        VerifySuccessResultTime(5, result);
        
        result = await _timeEngineController.GetTimeDistance(1, "StationB2", "Station1");
        VerifySuccessResultTime(5, result);
        
        result = await _timeEngineController.GetTimeDistance(1, "Station1", "Station1");
        VerifySuccessResultTime(0, result);
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task GetTimeItineraryError()
    {
        if (_timeEngineController == null)
            Assert.False(true);
        
        IActionResult result = await _timeEngineController.GetTimeDistance(1, "StationF2", "StationB2");
        result.Should().BeOfType<NotFoundObjectResult>();
        
        result = await _timeEngineController.GetTimeDistance(2, "Station1", "Station5");
        result.Should().BeOfType<NotFoundObjectResult>();
        
        result = await _timeEngineController.GetTimeDistance(1, "", "Station5");
        result.Should().BeOfType<BadRequestObjectResult>();
        
        result = await _timeEngineController.GetTimeDistance(1, "Station1", "");
        result.Should().BeOfType<BadRequestObjectResult>();
        
        result = await _timeEngineController.GetTimeDistance(-1, "Station1", "Station5");
        result.Should().BeOfType<BadRequestObjectResult>();
    }
}