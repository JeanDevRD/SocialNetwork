
namespace SocialNetwork.Core.Application.ViewModels.FriendRequest
{
    public class FriendRequestIndexViewModel
    {
        public List<FriendRequestViewModel> PendingRequests { get; set; } = new();
        public List<FriendRequestViewModel> SentRequests { get; set; } = new();
    }
}
