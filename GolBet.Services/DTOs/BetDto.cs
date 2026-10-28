using GolBet.Entities.Enums;



namespace GolBet.Services.DTOs;



/// <summary>A bet as "Mis apuestas" shows it. Flat, two hops deep.</summary> 

public class BetDto

{

    public int Id { get; set; }



    // Flattened: Bet.Match.HomeTeam.Name -> Match + HomeTeam + Name 

    public string MatchHomeTeamName { get; set; } = null!;

    public string MatchAwayTeamName { get; set; } = null!;

    public DateTime MatchDate { get; set; }          // UTC; the view converts 



    public BetPick Pick { get; set; }

    public decimal Amount { get; set; }

    public decimal OddsAtPlacement { get; set; }

    public decimal PotentialPayout { get; set; }     // Amount x OddsAtPlacement 

    public BetStatus Status { get; set; }

}