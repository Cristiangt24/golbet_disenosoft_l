using GolBet.Entities;
using GolBet.Entities.Enums;

namespace GolBet.Repositories.Interface;

public interface IMatchRepository : IGenericRepository<Match>
{
    Task<IEnumerable<Match>> GetAllWithTeamsAsync(MatchStatus? status = null);
    Task<Match?> GetByIdWithDetailsAsync(int id);
}

