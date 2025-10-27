
namespace SocialNetwork.Core.Domain.Entities
{
    public class FriendRequest : CommonEntity<int>
    {
        public required string SenderId { get; set; }
        public required string ReceiverId { get; set; }
        public required string Status { get; set; } 
        public required DateTime RequestedAt { get; set; }
    }
}

