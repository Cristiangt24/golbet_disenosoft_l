using AutoMapper;
using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interface;
using GolBet.Repositories.Interfaces;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GolBet.Services.Implementations;

public class BetService : IBetService

{

    private readonly IBetRepository _betRepository;

    private readonly IMatchRepository _matchRepository;

    private readonly UserManager<AppUser> _userManager;

    private readonly IMapper _mapper;



    public BetService(

        IBetRepository betRepository,

        IMatchRepository matchRepository,

        UserManager<AppUser> userManager,

        IMapper mapper)

    {

        _betRepository = betRepository;

        _matchRepository = matchRepository;

        _userManager = userManager;

        _mapper = mapper;

    }



    public async Task PlaceBetAsync(string userId, BetFormDto dto)

    {

        // Rule #1: the match must exist, be Scheduled, and lie in the future 

        var match = await _matchRepository.GetByIdAsync(dto.MatchId);

        if (match is null || !match.IsActive)

            throw new InvalidOperationException("El partido no existe.");



        if (match.Status != MatchStatus.Scheduled || match.Date <= DateTime.UtcNow)

            throw new InvalidOperationException(

                "Este partido ya no acepta apuestas.");



        // Rule #3: same pick on the same match only once 

        if (await _betRepository.ExistsAsync(dto.MatchId, userId, dto.Pick))

            throw new InvalidOperationException(

                "Ya tienes una apuesta con esta selección en este partido.");



        // Rule #2: amount must not exceed the balance 

        var user = await _userManager.FindByIdAsync(userId)

            ?? throw new InvalidOperationException("Usuario no encontrado.");



        if (dto.Amount > user.Balance)

            throw new InvalidOperationException(

                $"Saldo insuficiente: tienes {user.Balance:N2} FutCoins.");



        // Rule #4: freeze the odds of the chosen pick 

        var oddsAtPlacement = dto.Pick switch

        {

            BetPick.Home => match.HomeOdds,

            BetPick.Draw => match.DrawOdds,

            BetPick.Away => match.AwayOdds,

            _ => throw new InvalidOperationException("Selección inválida.")

        };



        // Rule #2 (second half): deduct the stake... 

        user.Balance -= dto.Amount;



        // ...and record the bet. ONE SaveChanges -> ONE transaction: 

        // the tracked user update and the new bet travel together. 

        var bet = new Bet

        {

            MatchId = dto.MatchId,

            UserId = userId,

            Pick = dto.Pick,

            Amount = dto.Amount,

            OddsAtPlacement = oddsAtPlacement,

            Status = BetStatus.Pending

        };



        await _betRepository.AddAsync(bet);   // SaveChangesAsync happens here 

    }



    public async Task<IEnumerable<BetDto>> GetMyBetsAsync(string userId)

        => _mapper.Map<IEnumerable<BetDto>>(await _betRepository.GetByUserAsync(userId));

}