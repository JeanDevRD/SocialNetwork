using AutoMapper;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork_Infrastructure.Identity.Entities;

namespace SocialNetwork_Infrastructure.Identity.Mappers.EntityToDto
{
    public class CreateUserDtoMappingProfile : Profile
    {
        public CreateUserDtoMappingProfile()
        {
            CreateMap<CreateUserDto, UserEntity>()
              .ForMember(p => p.PasswordHash, opt => opt.Ignore())
              .ReverseMap();
        }
    }
}
