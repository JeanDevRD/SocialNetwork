using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Domain.Entities;
namespace SocialNetwork.Core.Application.Mappers.EntityToDto
{
    public class PostDtoMappingProfile : Profile
    {
        public PostDtoMappingProfile()
        {
            CreateMap<Post, PostDto>();
        }
    }
}
