using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Game;
using SocialNetwork.Core.Domain.Entities;
namespace SocialNetwork.Core.Application.Mappers.EntityToDto
{
    public class GameDtoMappingProfile : Profile
    {
        public GameDtoMappingProfile() 
        { 
         CreateMap<Game,GameDto>()
                .ReverseMap();
        }

    }
}
