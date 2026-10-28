using System.ComponentModel.DataAnnotations;
using GolBet.Entities.Enums;
namespace GolBet.Services.DTOs;
/// <summary>What the betting panel sends. Who bets comes from the session.</summary> 

public class BetFormDto

{

    public int MatchId { get; set; }
    public BetPick Pick { get; set; }

    [Display(Name = "Monto")]

    [Range(1, 9_999_999_999, ErrorMessage = "El monto debe ser mayor que cero")]

    public decimal Amount { get; set; }

}