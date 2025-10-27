using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Reaction
{
    public class DeleteReactionViewModel
    {
        public required string UserId { get; set; }
        public required int PostId { get; set; }

    }
}
