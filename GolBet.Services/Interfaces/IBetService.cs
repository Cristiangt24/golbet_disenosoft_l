using GolBet.Services.DTOs;
namespace GolBet.Services.Interfaces;

public interface IBetService

{
    /// <summary>Places a bet for the given user. Throws on business rule violations.</summary> 
    Task PlaceBetAsync(string userId, BetFormDto dto);
    /// <summary>The user's betting history, newest first.</summary> 

    Task<IEnumerable<BetDto>> GetMyBetsAsync(string userId);
}