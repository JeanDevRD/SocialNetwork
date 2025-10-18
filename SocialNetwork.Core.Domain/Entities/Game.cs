using System;
using System.Collections;

namespace SocialNetwork.Core.Domain.Entities
{
    public class Game : CommonEntity<int>
    {
        public required int Player1Id { get; set; }
        public required int Player2Id { get; set; }

        public User? Player1 { get; set; }
        public User? Player2 { get; set; }

        public required DateTime Started { get; set; }
        public required DateTime  Ended { get; set; }
        public required string Status { get; set; } 
        public required User Winner { get; set; }

        public ICollection<Ship>? Ships { get; set; }
        public ICollection<Attack>? Attacks { get; set; }
    }
}
