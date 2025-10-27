
namespace SocialNetwork.Core.Application.ViewModels.FriendShip
{
    public class FriendsIndexViewModel
    {
        public List<FriendPostViewModel> FriendPosts { get; set; } = new();
        public List<FriendViewModel> Friends { get; set; } = new();
    }
}
