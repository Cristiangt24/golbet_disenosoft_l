// GolBet.Services/DTOs/MatchFormDto.cs
using System.ComponentModel.DataAnnotations;

namespace GolBet.Services.DTOs;

public class MatchFormDto
{
    public int Id { get; set; }

    [Display(Name = "Equipo local")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione el equipo local")]
    public int HomeTeamId { get; set; }

    [Display(Name = "Equipo visitante")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione el equipo visitante")]
    public int AwayTeamId { get; set; }

    [Display(Name = "Fecha y hora (hora de Colombia)")]
    public DateTime Date { get; set; } = DateTime.Today.AddDays(7).AddHours(19);

    [Display(Name = "Cuota local (1)")]
    [Range(1.01, 999.99, ErrorMessage = "La cuota debe ser mayor que 1.00")]
    public decimal HomeOdds { get; set; } = 2.00m;

    [Display(Name = "Cuota empate (X)")]
    [Range(1.01, 999.99, ErrorMessage = "La cuota debe ser mayor que 1.00")]
    public decimal DrawOdds { get; set; } = 3.00m;

    [Display(Name = "Cuota visitante (2)")]
    [Range(1.01, 999.99, ErrorMessage = "La cuota debe ser mayor que 1.00")]
    public decimal AwayOdds { get; set; } = 3.50m;
}

