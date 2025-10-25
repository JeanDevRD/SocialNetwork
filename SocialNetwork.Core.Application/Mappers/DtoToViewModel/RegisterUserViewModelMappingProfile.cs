using AutoMapper;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Core.Application.ViewModels.User;

namespace SocialNetwork.Core.Application.Mappers.DtoToViewModel
{
    public class RegisterUserViewModelMappingProfile : Profile
    {
        public RegisterUserViewModelMappingProfile()
        {
            CreateMap<UserRegisterViewModel, CreateUserDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Profile, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ReverseMap();

        }
    }
}
