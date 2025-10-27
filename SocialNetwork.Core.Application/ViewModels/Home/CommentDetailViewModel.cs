using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Home
{
    public class CommentDetailViewModel
    {
        public int Id { get; set; }
        public required string Content { get; set; } 
        public DateTime Created { get; set; }
        public required string AuthorId { get; set; }
        public required string AuthorName { get; set; } 
        public required string AuthorProfile { get; set; } 
        public int? ParentCommentId { get; set; }
        public List<CommentDetailViewModel> Replies { get; set; } = new();
    }
}
