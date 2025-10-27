using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.User;

namespace SocialNetwork.Core.Application.DTOs.Friendship
{
    public class FriendShipDto : CommonEntityDto<int>
    {
        public UserDto? User { get; set; }
        public UserDto? Friend { get; set; }
        public required string UserId { get; set; }
        public required string FriendId { get; set; }
        public required DateTime Created { get; set; }
    }
}
