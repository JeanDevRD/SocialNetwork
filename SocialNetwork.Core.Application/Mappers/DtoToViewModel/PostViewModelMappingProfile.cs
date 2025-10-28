using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.ViewModels.Post;


namespace SocialNetwork.Core.Application.Mappers.DtoToViewModel
{
    public class PostViewModelMappingProfile : Profile
    {
        public PostViewModelMappingProfile() 
        {

            CreateMap<CreatePostViewModel, PostDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => 0))
               .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageFile))
               .ReverseMap();

            CreateMap<EditPostViewModel, PostDto>()
                .ForMember(dest => dest.Created, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ReverseMap();

            CreateMap<DeletePostViewModel, PostDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PostId))
                .ReverseMap();

        }
    }
}
