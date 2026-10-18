using GolBet.Entities;
using GolBet.Entities.Enums;

namespace GolBet.Repositories.Interface;

public interface IMatchRepository : IGenericRepository<Match>
{
    Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null, string? search = null); // Add to search by team name
    Task<Match?> GetByIdWithDetailsAsync(int id);
}

