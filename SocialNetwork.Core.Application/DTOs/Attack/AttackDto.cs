using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.Game;
using SocialNetwork.Core.Application.DTOs.User;

namespace SocialNetwork.Core.Application.DTOs.Attack
{
    public class AttackDto : CommonEntityDto<int>
    {
        public required int GameId { get; set; }
        public required string AttackerId { get; set; }
        public GameDto? Game { get; set; }
        public UserDto? Attacker { get; set; }
        public required int Row { get; set; }
        public required int Column { get; set; }
        public required bool Hit { get; set; }
        public required DateTime Attacked { get; set; }
    }
}
