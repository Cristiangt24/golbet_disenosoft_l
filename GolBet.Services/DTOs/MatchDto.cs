using GolBet.Entities.Enums;

namespace GolBet.Services.DTOs;

/// <summary>
/// Read model for the match board. Flat: no navigation properties.
/// </summary>
public class MatchDto
{
    public int Id { get; set; }

    public DateTime Date { get; set; } // UTC; the view converts
    public MatchStatus Status { get; set; }

    // Flattened from Match.HomeTeam / Match.AwayTeam (AutoMapper convention)
    public string HomeTeamName { get; set; } = null!;
    public string? HomeTeamCrestUrl { get; set; }
    public string AwayTeamName { get; set; } = null!;
    public string? AwayTeamCrestUrl { get; set; }

    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }

    public decimal HomeOdds { get; set; }
    public decimal DrawOdds { get; set; }
    public decimal AwayOdds { get; set; }
}

