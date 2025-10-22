using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.DTOs.Reaction
{
    public class ReactionDto : CommonEntityDto<int>
    {
        public required string Type { get; set; }
        public required DateTime Created { get; set; }

        public required int UserId { get; set; }
        public required int PostId { get; set; }

        public UserDto? Author { get; set; }
        public PostDto? Post { get; set; }
    }
}
