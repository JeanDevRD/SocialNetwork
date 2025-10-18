
namespace SocialNetwork.Core.Domain.Entities
{
    public class FriendRequest : CommonEntity<int>
    {
        public required int SenderId { get; set; }
        public required int ReceiverId { get; set; }
        public User? Sender { get; set; }
        public User? Receiver { get; set; }
        public required string Status { get; set; } 
        public required DateTime RequestedAt { get; set; }
    }
}

