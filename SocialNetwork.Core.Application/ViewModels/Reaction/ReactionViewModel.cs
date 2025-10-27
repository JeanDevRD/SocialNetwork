using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Reaction
{
    public class ReactionViewModel
    {
        public required int Id { get; set; }
        public string? Type { get; set; }
        public DateTime? Created { get; set; }

        public string? UserId { get; set; }
        public required int PostId { get; set; }

        public UserDto? Author { get; set; }
        public PostDto? Post { get; set; }
    }
}
