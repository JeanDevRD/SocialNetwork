using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.FriendRequest
{
    public class FriendRequestViewModel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string? SenderUserName { get; set; } 
        public string? SenderProfile { get; set; } 
        public int ReceiverId { get; set; }
        public string? ReceiverUserName { get; set; } 
        public string? ReceiverProfile { get; set; } 
        public string? Status { get; set; } 
        public DateTime RequestedAt { get; set; }
        public int CommonFriends { get; set; }
    }
}
