
namespace SocialNetwork.Core.Domain.Entities
{
    public class FriendShip : CommonEntity<int>
    {
        public required int UserId { get; set; }
        public required int FriendId { get; set; }
        public required DateTime Created { get; set; }
    }
}
