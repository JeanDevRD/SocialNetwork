using AutoMapper;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Core.Application.ViewModels.User;

namespace SocialNetwork.Core.Application.Mappers.DtoToViewModel
{
    public class RequestPasswordResetViewModelMappingProfile : Profile
    {
        public RequestPasswordResetViewModelMappingProfile()
        {
            CreateMap<UserResponseDto, ForgotPasswordRequestViewModel>().ReverseMap();
        }
    }
}
