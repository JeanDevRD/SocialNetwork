
namespace SocialNetwork.Core.Domain.Entities
{
    public class FriendShip : CommonEntity<int>
    {
        public required string UserId { get; set; }
        public required string FriendId { get; set; }
        public required DateTime Created { get; set; }
    }
}
