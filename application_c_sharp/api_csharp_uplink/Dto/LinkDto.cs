using System.ComponentModel.DataAnnotations;

namespace api_csharp_uplink.Dto;

public class LinkDto
{
    [Required]
    [RegularExpression(@"\S+", ErrorMessage = "NameStation1 cannot be empty or whitespace.")]
    public string NameStation1 { get; set; } = "";
    
    [Required]
    [RegularExpression(@"\S+", ErrorMessage = "NameStation2 cannot be empty or whitespace.")]    
    public string NameStation2 { get; set; } = "";
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "LineNumber must be greater than 0")]
    public int LineNumber { get; set; }

    [Required]
    [RegularExpression("FORWARD|BACKWARD", ErrorMessage = "Orientation must be FORWARD or BACKWARD")]
    public string Orientation { get; set; } = "";
}