using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Friendship;
using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.ViewModels.FriendShip;

namespace SocialNetwork.Core.Application.Mappers.DtoToViewModel
{
    public class FriendShipViewModelMappingProfile : Profile
    {
        public FriendShipViewModelMappingProfile()
        {
            CreateMap<PostDto, FriendPostViewModel>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.Username : string.Empty))
                .ForMember(dest => dest.AuthorProfile, opt => opt.MapFrom(src => src.Author != null ? src.Author.Profile : string.Empty));

            CreateMap<FriendShipDto, FriendViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FriendId.ToString()))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Friend != null ? src.Friend.FirstName : string.Empty))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Friend != null ? src.Friend.LastName : string.Empty))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Friend != null ? src.Friend.Username : string.Empty))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.Friend != null ? src.Friend.Profile : string.Empty));
        }
    }
}
