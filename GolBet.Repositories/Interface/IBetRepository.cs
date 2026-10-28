using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interface;

namespace GolBet.Repositories.Interfaces;

public interface IBetRepository : IGenericRepository<Bet>

{
    /// <summary>Does this user already have this exact pick on this match?</summary> 
    Task<bool> ExistsAsync(int matchId, string userId, BetPick pick);
    /// <summary>All bets of a user, newest first, with match and teams loaded.</summary> 
    Task<IEnumerable<Bet>> GetByUserAsync(string userId);

}