using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Attack;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Mappers.EntityToDto
{
    public class AttackDtoMappingProfile : Profile
    {
        public AttackDtoMappingProfile()
        {
            CreateMap<Attack, AttackDto>();
        }
    }
}
