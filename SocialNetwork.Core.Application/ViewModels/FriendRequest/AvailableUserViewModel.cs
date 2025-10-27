using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.FriendRequest
{
    public class AvailableUserViewModel
    {
        public string? Id { get; set; } 
        public string? Username { get; set; } 
        public string? Profile { get; set; } 
        public int CommonFriends { get; set; }
    }
}
