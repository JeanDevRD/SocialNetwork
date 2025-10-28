using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Comment;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Application.Mappers.EntityToDto
{
    public class CommentDtoMappingProfile : Profile
    {
        public CommentDtoMappingProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ReverseMap();
        }
    }
}
