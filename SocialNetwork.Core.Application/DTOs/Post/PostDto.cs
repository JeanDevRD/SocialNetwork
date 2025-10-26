using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.User;
using SocialNetwork.Core.Application.DTOs.Comment; // Corregido: Importa el tipo CommentDto
using SocialNetwork.Core.Application.DTOs.Reaction; 
using SocialNetwork.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
