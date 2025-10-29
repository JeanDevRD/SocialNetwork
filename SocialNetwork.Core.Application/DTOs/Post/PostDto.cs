using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Core.Application.DTOs.Comment; 
using SocialNetwork.Core.Application.DTOs.Reaction; 

namespace SocialNetwork.Core.Application.DTOs.Post
{
    public class PostDto : CommonEntityDto<int>
    {
        public required string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public required DateTime Created { get; set; }

        public required string UserId { get; set; }
        public UserDto? Author { get; set; }

        public ICollection<CommentDto>? Comments { get; set; }
        public ICollection<ReactionDto>? Reactions { get; set; }
    }
}
