using AutoMapper;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork_Infrastructure.Identity.Entities;

namespace SocialNetwork_Infrastructure.Identity.Mappers.EntityToDto
{
    public class UserToLoginDtoMappingProfile : Profile
    {
        public UserToLoginDtoMappingProfile()
        {
            CreateMap<UserEntity, LoginResponseDto>()
                .ForMember(U => U.IsVerified, opt => opt.MapFrom(U => U.EmailConfirmed))
                .ReverseMap();
        }
    }
}
