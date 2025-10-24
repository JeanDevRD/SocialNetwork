using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Ship;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Mappers.EntityToDto
{
    public class ShipDtoMappingProfile : Profile
    {
        public ShipDtoMappingProfile() 
        { 
         CreateMap<Ship, ShipDto>();
        }
    }
}
