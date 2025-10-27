using AutoMapper;
using SocialNetwork.Core.Application.DTOs.FriendRequest;
using SocialNetwork.Core.Application.ViewModels.FriendRequest;
namespace SocialNetwork.Core.Application.Mappers.DtoToViewModel
{
    public class FriendRequestDtoMappingProfile : Profile
    {
        public FriendRequestDtoMappingProfile()
        {
            CreateMap<FriendRequestDto, FriendRequestViewModel>()
                .ForMember(dest => dest.SenderUserName, opt => opt.MapFrom(src => src.Sender != null ? src.Sender.Username : string.Empty))
                .ForMember(dest => dest.SenderProfile, opt => opt.MapFrom(src => src.Sender != null ? src.Sender.Profile : string.Empty))
                .ForMember(dest => dest.ReceiverUserName, opt => opt.MapFrom(src => src.Receiver != null ? src.Receiver.Username : string.Empty))
                .ForMember(dest => dest.ReceiverProfile, opt => opt.MapFrom(src => src.Receiver != null ? src.Receiver.Profile : string.Empty))
                .ForMember(dest => dest.CommonFriends, opt => opt.Ignore());
        }
    }
}
