using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.FriendShip
{
    public class UserPostsViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserProfile { get; set; } = string.Empty;
        public List<FriendPostViewModel> Posts { get; set; } = new();
    }
}
