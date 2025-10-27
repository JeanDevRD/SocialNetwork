using AutoMapper;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Infrastructure.Identity.Entities;

namespace SocialNetwork.Infrastructure.Identity.Mappers.EntityToDto
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
