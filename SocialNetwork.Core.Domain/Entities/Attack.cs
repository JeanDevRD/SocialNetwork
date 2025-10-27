
namespace SocialNetwork.Core.Domain.Entities
{
    public class Attack : CommonEntity<int>
    {
        public required int GameId { get; set; } 
        public required string AttackerId { get; set; }  
        public Game? Game { get; set; }
        public required int Row { get; set; }
        public required int Column { get; set; }
        public required bool Hit { get; set; }
        public required DateTime Attacked { get; set; }
    }
}
