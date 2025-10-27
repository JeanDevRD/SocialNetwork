using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Home
{
    public class PostDetailViewModel
    {
        public int Id { get; set; }
        public required string Content { get; set; } 
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime Created { get; set; }
        public required string AuthorId { get; set; } 
        public required string AuthorName { get; set; }
        public required string AuthorProfile { get; set; }
        public List<CommentDetailViewModel> Comments { get; set; } = new();
        public List<ReactionDetailViewModel> Reactions { get; set; } = new();
        public bool CurrentUserLiked { get; set; }
        public bool CurrentUserDisliked { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
    }
}
