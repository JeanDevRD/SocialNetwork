using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Comment;
using SocialNetwork.Core.Application.ViewModels.Comment;
namespace SocialNetwork.Core.Application.Mappers.DtoToViewModel
{
    public class CommentViewModelMappingProfile : Profile
    {
        public CommentViewModelMappingProfile()
        {
            CreateMap<CreateCommentViewModel, CommentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created ?? DateTime.Now))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.ParentCommentId, opt => opt.MapFrom(src => src.ParentCommentId))
                .ReverseMap();

          
            CreateMap<CommentDto, EditCommentViewModel>()
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ReverseMap();


            CreateMap<CommentDto, DeleteCommentViewModel>()
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
