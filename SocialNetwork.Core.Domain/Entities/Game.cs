
namespace SocialNetwork.Core.Domain.Entities
{
    public class Game : CommonEntity<int>
    {
        public required string Player1Id { get; set; }
        public required string Player2Id { get; set; }

        public required DateTime Started { get; set; }
        public DateTime? Ended { get; set; }  
        public required string Status { get; set; }  
        public string? WinnerId { get; set; }  
        public required string CurrentTurnPlayerId { get; set; }  

        public ICollection<Ship>? Ships { get; set; }
        public ICollection<Attack>? Attacks { get; set; }
    }
}

