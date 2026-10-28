using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Data;
using GolBet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace GolBet.Repositories.Implementations;

public class BetRepository : GenericRepository<Bet>, IBetRepository
{
    public BetRepository(AppDbContext context) : base(context) { }

    public async Task<bool> ExistsAsync(int matchId, string userId, BetPick pick)

        => await _dbSet.AnyAsync(b =>

               b.MatchId == matchId && b.UserId == userId && b.Pick == pick);

    public async Task<IEnumerable<Bet>> GetByUserAsync(string userId)

        => await _dbSet

            .Where(b => b.UserId == userId)
            .Include(b => b.Match)
                .ThenInclude(m => m.HomeTeam)
            .Include(b => b.Match)
                .ThenInclude(m => m.AwayTeam)
            .OrderByDescending(b => b.CreatedDate)
            .AsNoTracking()
            .ToListAsync();

}