using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Reaction;
using SocialNetwork.Core.Application.ViewModels.Reaction;

namespace SocialNetwork.Core.Application.Mappers.DtoToViewModel
{
    public class ReactionViewModelMappingProfile : Profile
    {
        public ReactionViewModelMappingProfile()
        {
            CreateMap<ReactionDto, ReactionViewModel>()
                .ReverseMap();

        }
    }
}
