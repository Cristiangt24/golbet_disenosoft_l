using AutoMapper;
using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interface;
using GolBet.Repositories.Interfaces;
using GolBet.Services.DTOs;
using GolBet.Services.Helpers;
using GolBet.Services.Interfaces;

namespace GolBet.Services.Implementations;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;
    private readonly IBetRepository _betRepository;      // nuevo 


    public MatchService(IMatchRepository matchRepository, IBetRepository betRepository, IMapper mapper)
    {
        _matchRepository = matchRepository;
        _betRepository = betRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MatchDto>> GetBoardAsync(MatchStatus? status = null, string? search = null) // Add search parameter
    {
        var matches = await _matchRepository.GetAllWithTeamsAsync(status, search);
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

    public async Task StartMatchAsync(int id)

    {

        var match = await _matchRepository.GetByIdAsync(id)

            ?? throw new KeyNotFoundException($"Match {id} not found.");



        if (match.Status != MatchStatus.Scheduled)

            throw new InvalidOperationException(

                "Solo un partido programado puede iniciarse.");



        match.Status = MatchStatus.InProgress;   // betting closes (rule #1 reads this) 

        await _matchRepository.UpdateAsync(match);

    }



    public async Task<ResolutionSummaryDto> CloseMatchAsync(MatchResultFormDto dto)

    {

        var match = await _matchRepository.GetByIdAsync(dto.MatchId)

            ?? throw new KeyNotFoundException($"Match {dto.MatchId} not found.");



        // State machine guards (first idempotence belt) 

        if (match.Status == MatchStatus.Finished)

            throw new InvalidOperationException(

                "Este partido ya fue cerrado y sus apuestas resueltas.");



        if (match.Status != MatchStatus.InProgress)

            throw new InvalidOperationException(

                "El partido debe estar «En juego» para cerrarse. Inícialo primero.");



        // 1. Record the final score and the new state 

        match.HomeGoals = dto.HomeGoals;

        match.AwayGoals = dto.AwayGoals;

        match.Status = MatchStatus.Finished;



        // 2. Compute the winning pick from the score 

        var winningPick = GetWinningPick(dto.HomeGoals, dto.AwayGoals);



        // 3. Resolve every PENDING bet (second idempotence belt: only Pending arrive) 

        var pendingBets = await _betRepository.GetPendingByMatchAsync(match.Id);

        var summary = new ResolutionSummaryDto { ResolvedBets = pendingBets.Count };



        foreach (var bet in pendingBets)

        {

            if (bet.Pick == winningPick)

            {

                bet.Status = BetStatus.Won;

                var payout = bet.Amount * bet.OddsAtPlacement;   // FROZEN odds pay 

                bet.User.Balance += payout;                      // the win lands 

                summary.WonBets++;

                summary.TotalPaid += payout;

            }

            else

            {

                bet.Status = BetStatus.Lost;   // stake was deducted at placement 

            }

        }



        // 4. ONE save: the match, every bet and every balance — ONE transaction 

        await _matchRepository.UpdateAsync(match);



        return summary;

    }



    private static BetPick GetWinningPick(int homeGoals, int awayGoals)

        => homeGoals > awayGoals ? BetPick.Home

         : homeGoals < awayGoals ? BetPick.Away

         : BetPick.Draw;



}
