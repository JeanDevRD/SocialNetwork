using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Reaction;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Mappers.EntityToDto
{
    public class ReactionDtoMappingProfile : Profile
    {
        public ReactionDtoMappingProfile()
        {
            CreateMap<Reaction,ReactionDto>();
        } 
    }
}
