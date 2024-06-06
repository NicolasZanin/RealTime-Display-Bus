using System.ComponentModel.DataAnnotations;

namespace api_csharp_uplink.Dto;

public class PositionBusDto
{
    [Required]
    public PositionDto Position { get; set; }
    
    [Range(0, int.MaxValue)]
    public int DevEuiNumber { get; set; }
}