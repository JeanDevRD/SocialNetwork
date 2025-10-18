
namespace SocialNetwork.Core.Domain.Entities
{
    public class User : CommonEntity<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Profile { get; set; }
        public required bool IsActive { get; set; }


        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }

  
        public ICollection<FriendShip>? Friends { get; set; }      
        public ICollection<FriendShip>? FriendOf { get; set; }       

      
        public ICollection<FriendRequest>? SentRequests { get; set; }
        public ICollection<FriendRequest>? ReceivedRequests { get; set; }

     
        public ICollection<Game>? Player1 { get; set; }
        public ICollection<Game>? Player2 { get; set; }
    }
}
