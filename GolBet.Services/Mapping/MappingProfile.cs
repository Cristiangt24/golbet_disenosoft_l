using AutoMapper;
using GolBet.Entities;
using GolBet.Services.DTOs;

namespace GolBet.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Flattening by convention:
        // MatchDto.HomeTeamName  <- Match.HomeTeam.Name
        // MatchDto.AwayTeamCrestUrl <- Match.AwayTeam.CrestUrl
        CreateMap<Match, MatchDto>();
    }
}

