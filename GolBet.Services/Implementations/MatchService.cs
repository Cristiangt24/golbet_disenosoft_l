using AutoMapper;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interface;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;

namespace GolBet.Services.Implementations;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;

    public MatchService(IMatchRepository matchRepository, IMapper mapper)
    {
        _matchRepository = matchRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null)
    {
        var matches = await _matchRepository.GetAllWithTeamsAsync(status);
        return _mapper.Map<IEnumerable<MatchDto>>(matches);
    }
    // GolBet.Services/Implementations/MatchService.cs  (agregar)
    public async Task<MatchDetailDto?> GetDetailAsync(int id)
    {
        var match = await _matchRepository.GetByIdWithDetailsAsync(id);
        return match is null ? null : _mapper.Map<MatchDetailDto>(match);
    }

}
