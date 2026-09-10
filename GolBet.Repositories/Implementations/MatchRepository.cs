using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Data;
using GolBet.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GolBet.Repositories.Implementations;

public class MatchRepository : GenericRepository<Match>, IMatchRepository
{
    public MatchRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null)
    {
        var query = _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Where(m => m.IsActive)
            .AsNoTracking()
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(m => m.Status == status.Value);

        return await query.OrderBy(m => m.Date).ToListAsync();
    }

    public async Task<Match?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.Bets)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
}

