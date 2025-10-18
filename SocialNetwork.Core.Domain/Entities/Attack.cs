
namespace SocialNetwork.Core.Domain.Entities
{
    public class Attack : CommonEntity<int>
    {
        public Game? Game { get; set; }
        public User? Attacker { get; set; }
        public required int Row { get; set; }
        public required int Column { get; set; }
        public required bool Hit { get; set; }
        public required DateTime Attacked { get; set; }
    }
}
