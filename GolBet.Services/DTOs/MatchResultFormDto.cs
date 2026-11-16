using System.ComponentModel.DataAnnotations;


namespace GolBet.Services.DTOs;

/// <summary>The final score the Admin submits to close a match.</summary> 

public class MatchResultFormDto

{

    public int MatchId { get; set; }



    [Display(Name = "Goles del local")]

    [Range(0, 99, ErrorMessage = "Los goles deben estar entre 0 y 99")]

    public int HomeGoals { get; set; }

    [Display(Name = "Goles del visitante")]

    [Range(0, 99, ErrorMessage = "Los goles deben estar entre 0 y 99")]

    public int AwayGoals { get; set; }

}