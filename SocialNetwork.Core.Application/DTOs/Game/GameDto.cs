using SocialNetwork.Core.Application.DTOs.Attack;
using SocialNetwork.Core.Application.DTOs.CommonEntity;
using SocialNetwork.Core.Application.DTOs.Ship;
using SocialNetwork.Core.Application.DTOs.User;


namespace SocialNetwork.Core.Application.DTOs.Game
{
    public class GameDto : CommonEntityDto<int>
    {
        public required int Player1Id { get; set; }
        public required int Player2Id { get; set; }

        public UserDto? Player1 { get; set; }
        public UserDto? Player2 { get; set; }

        public required DateTime Started { get; set; }
        public DateTime? Ended { get; set; }
        public required string Status { get; set; }
        public int? WinnerId { get; set; }
        public UserDto? Winner { get; set; }
        public required int CurrentTurnPlayerId { get; set; }

        public ICollection<ShipDto>? Ships { get; set; }
        public ICollection<AttackDto>? Attacks { get; set; }
    }
}
