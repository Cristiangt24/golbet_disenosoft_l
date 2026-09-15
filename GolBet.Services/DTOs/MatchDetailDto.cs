namespace GolBet.Services.DTOs;

/// <summary>
/// Read model for the match detail page.
/// Inherits everything the board shows and adds detail-only data.
/// </summary>
public class MatchDetailDto : MatchDto
{
    /// <summary>How many bets have been placed on this match.</summary>
    public int TotalBets { get; set; }
}
