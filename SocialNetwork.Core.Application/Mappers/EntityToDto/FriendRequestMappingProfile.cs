using AutoMapper;
using SocialNetwork.Core.Application.DTOs.FriendRequest;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Mappers.EntityToDto
{
    public class FriendRequestMappingProfile : Profile
    {
        public FriendRequestMappingProfile()
        {
            CreateMap<FriendRequest, FriendRequestDto>()
                .ReverseMap();
        }
    }
}
