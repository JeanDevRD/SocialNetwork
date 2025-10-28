using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Friendship;
using SocialNetwork.Core.Domain.Entities;
namespace SocialNetwork.Core.Application.Mappers.EntityToDto
{
    public class FriendShipDtoMappingProfile : Profile
    {
        public FriendShipDtoMappingProfile()
        {
            CreateMap<FriendShip, FriendShipDto>()
                .ReverseMap();
        }
    }
}
