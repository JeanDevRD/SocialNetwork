
using SocialNetwork.Core.Application.DTOs.Comment;
using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.FriendRequest;
using SocialNetwork.Core.Application.DTOs.Friendship;
using SocialNetwork.Core.Application.DTOs.Game;
using SocialNetwork.Core.Application.DTOs.Post;
using SocialNetwork.Core.Application.DTOs.Reaction;

namespace SocialNetwork.Core.Application.DTOs.User
{
    public class UserDto 
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Profile { get; set; }
        public required bool IsActive { get; set; }
        public required bool IsVerified { get; set; }



    }
}
