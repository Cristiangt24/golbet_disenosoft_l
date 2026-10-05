// GolBet.Services/DTOs/TeamDto.cs
namespace GolBet.Services.DTOs;

public class TeamDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public string? CrestUrl { get; set; }
    public bool IsActive { get; set; }
}
