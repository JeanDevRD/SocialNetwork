using AutoMapper;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork_Infrastructure.Identity.Entities;

namespace SocialNetwork_Infrastructure.Identity.Mappers.EntityToDto
{
    public class UserToEditMappingProfile : Profile
    {
        public UserToEditMappingProfile()
        {
            CreateMap<UserEntity, EditResponseDto>()
                .ForMember(U => U.IsVerified, opt => opt.MapFrom(U => U.EmailConfirmed))
                .ReverseMap();
        }
    }
}
