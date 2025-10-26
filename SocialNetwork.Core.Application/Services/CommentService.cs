using AutoMapper;
using SocialNetwork.Core.Application.DTOs.Comment;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Interfaces;

namespace SocialNetwork.Core.Application.Services
{
    public class CommentService : GenericService<CommentDto, Comment>, ICommentService
    {
        public CommentService(IGenericRepository<Comment> repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}
