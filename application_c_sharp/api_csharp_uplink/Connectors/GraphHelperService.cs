using System.Globalization;
using System.Text;
using api_csharp_uplink.Connectors.ExternalEntities;
using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;
using api_csharp_uplink.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace api_csharp_uplink.Connectors;

public class GraphHelperService(IOptions<GraphHopperSettings> graphHopperSettings) : IGraphHelper
{
    private readonly HttpClient _client = new();
    private readonly string _request = graphHopperSettings.Value.Url + "?key=" + graphHopperSettings.Value.Token;

    private static string ConvertPositionToRequestGraphHopper(Position position1, Position position2)
    {
        string[][] points =
        [
            [
                position1.Longitude.ToString(CultureInfo.CurrentCulture),
                position1.Latitude.ToString(CultureInfo.CurrentCulture)
            ],
            [
                position2.Longitude.ToString(CultureInfo.CurrentCulture),
                position2.Latitude.ToString(CultureInfo.CurrentCulture)
            ]
        ];
        
        var jsonToConvert = new
        {
            profile = "car",
            points,
            snap_preventions = new[]
            {
                "ferry"
            }
        };
        
        return JsonConvert.SerializeObject(jsonToConvert);
    }
    
    
    public async Task<TimeDistance> GetTimeAndDistance(Position position1, Position position2)
    {
        try
        {
            if (!graphHopperSettings.Value.Use) 
                return new TimeDistance(5, 5.0);
            
            string json = ConvertPositionToRequestGraphHopper(position1, position2);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            Console.WriteLine(_request);
            HttpResponseMessage response = await _client.PostAsync(_request, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            var jsonResult = JsonConvert.DeserializeObject<dynamic>(responseContent);
            if (jsonResult == null)
                throw new RequestExternalServiceException("Error in the request to the external service");

            int time = jsonResult["paths"][0]["time"] / 1000;
            double distance = jsonResult["paths"][0]["distance"];
                
            return new TimeDistance(time, distance);
        }
        catch (Exception e)
        {
            throw new RequestExternalServiceException("Error in the request to the external service : " + e.Message);
        }
    }
}