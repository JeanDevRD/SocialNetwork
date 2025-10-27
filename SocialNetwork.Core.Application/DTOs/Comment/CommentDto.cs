using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.DTOs.User;

namespace SocialNetwork.Core.Application.DTOs.Comment
{
    public class CommentDto : CommonEntityDto<int>
    {
        public required string Content { get; set; }
        public required DateTime Created { get; set; }

        public required string UserId { get; set; }
        public required int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        public UserDto? Author { get; set; }
        public PostDto? Post { get; set; }
        public CommentDto? ParentComment { get; set; }
        public ICollection<CommentDto>? Replies { get; set; }
    }
}
