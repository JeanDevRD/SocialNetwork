using SocialNetwork.Core.Application.DTOs.Comment;
using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Core.Application.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Comment
{
    public class CommentViewModel
    {
        public required int Id { get; set; }
        public required string Content { get; set; }
        public required DateTime Created { get; set; }

        public required string UserId { get; set; }
        public required int PostId { get; set; }
        public int? ParentCommentId { get; set; }

        public PostViewModel? Post { get; set; }
        public CommentViewModel? ParentComment { get; set; }
        public ICollection<CommentViewModel>? Replies { get; set; }
    }
}
