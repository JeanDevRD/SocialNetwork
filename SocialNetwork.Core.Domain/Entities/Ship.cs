
namespace SocialNetwork.Core.Domain.Entities
{
    public class Ship : CommonEntity<int>
    {
        public Game? Game { get; set; }
        public User? Owner { get; set; }
        public required int GameId { get; set; }  
        public required int OwnerId { get; set; } 
        public required int Size { get; set; }
        public required int StartRow { get; set; }  
        public required int StartColumn { get; set; }  
        public required string Direction { get; set; }  
        public required bool IsSunk { get; set; }
       
    }
}
