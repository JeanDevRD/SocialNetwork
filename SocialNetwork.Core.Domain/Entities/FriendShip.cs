
namespace SocialNetwork.Core.Domain.Entities
{
    public class FriendShip : CommonEntity<int>
    {
        public User? User { get; set; }    
        public User? Friend { get; set; }
        public required int UserId { get; set; }
        public required int FriendId { get; set; }
        public required DateTime Created { get; set; }
    }
}
