using System.ComponentModel.DataAnnotations;
using api_csharp_uplink.Entities;
using Newtonsoft.Json;

namespace api_csharp_uplink.Dto;

public class PositionBusDto
{
    [Required(ErrorMessage = "Position is required")] 
    public PositionDto Position { get; set; } = new();
    
    [Required]
    [RegularExpression(@"\S+", ErrorMessage = "DevEuiNumber cannot be empty or whitespace.")]
    public string DevEuiNumber { get; set; } = "";
}