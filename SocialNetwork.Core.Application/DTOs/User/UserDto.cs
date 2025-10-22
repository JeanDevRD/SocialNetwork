
using SocialNetwork.Core.Application.DTOs.Comment;
using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.FriendRequest;
using SocialNetwork.Core.Application.DTOs.Friendship;
using SocialNetwork.Core.Application.DTOs.Game;
using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.DTOs.Reaction;

namespace SocialNetwork.Core.Application.DTOs.User
{
    public class UserDto : CommonEntityDto<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Profile { get; set; }
        public required bool IsActive { get; set; }


        public ICollection<PostDto>? Posts { get; set; }
        public ICollection<CommentDto>? Comments { get; set; }
        public ICollection<ReactionDto>? Reactions { get; set; }


        public ICollection<FriendShipDto>? Friends { get; set; }
        public ICollection<FriendShipDto>? FriendOf { get; set; }


        public ICollection<FriendRequestDto>? SentRequests { get; set; }
        public ICollection<FriendRequestDto>? ReceivedRequests { get; set; }


        public ICollection<GameDto>? Player1 { get; set; }
        public ICollection<GameDto>? Player2 { get; set; }
        public ICollection<GameDto>? GamesWon { get; set; }
    }
}
