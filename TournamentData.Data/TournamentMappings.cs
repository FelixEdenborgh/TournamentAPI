using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentCore.Core.Dto;
using TournamentCore.Core.Entities;

namespace TournamentData.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings() 
        {
            CreateMap<TournamentEntities, TournamentDto>().ReverseMap();
            CreateMap<GameEntities, GameDto>().ReverseMap();
        }
    }
}
