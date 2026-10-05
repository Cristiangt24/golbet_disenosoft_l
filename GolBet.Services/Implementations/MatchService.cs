using AutoMapper;
using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interface;
using GolBet.Services.DTOs;
using GolBet.Services.Helpers;
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

    public async Task<MatchFormDto?> GetForEditAsync(int id)
    {
        var match = await _matchRepository.GetByIdAsync(id);
        if (match is null) return null;

        var dto = _mapper.Map<MatchFormDto>(match);
        dto.Date = dto.Date.ToColombiaTime();        // UTC (DB) -> local (form)
        return dto;
    }

    public async Task CreateAsync(MatchFormDto dto)
    {
        ValidateBusinessRules(dto);

        var match = _mapper.Map<Match>(dto);
        match.Date = dto.Date.ToUtcFromColombia();   // local (form) -> UTC (DB)
        await _matchRepository.AddAsync(match);
    }

    public async Task UpdateAsync(MatchFormDto dto)
    {
        ValidateBusinessRules(dto);

        var match = await _matchRepository.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Match {dto.Id} not found.");

        _mapper.Map(dto, match);
        match.Date = dto.Date.ToUtcFromColombia();
        await _matchRepository.UpdateAsync(match);
    }

    public async Task DeactivateAsync(int id)
        => await _matchRepository.DeactivateAsync(id);

    // ---- Business rules (Guía, regla #1 y afines) ----
    private static void ValidateBusinessRules(MatchFormDto dto)
    {
        if (dto.HomeTeamId == dto.AwayTeamId)
            throw new InvalidOperationException(
                "El equipo local y el visitante no pueden ser el mismo.");

        if (dto.Date.ToUtcFromColombia() <= DateTime.UtcNow)
            throw new InvalidOperationException(
                "La fecha del partido debe ser futura.");
    }


}
