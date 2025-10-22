using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.User;

namespace SocialNetwork.Core.Application.DTOs.FriendRequest
{
    public class FriendRequestDto : CommonEntityDto<int>
    {
        public required int SenderId { get; set; }
        public required int ReceiverId { get; set; }
        public UserDto? Sender { get; set; }
        public UserDto? Receiver { get; set; }
        public required string Status { get; set; }
        public required DateTime RequestedAt { get; set; }
    }
}
