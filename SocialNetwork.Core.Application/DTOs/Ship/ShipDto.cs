using SocialNetwork.Core.Application.DTOs.Game;
using SocialNetwork.Core.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.DTOs.Ship
{
    public class ShipDto
    {
        public GameDto? Game { get; set; }
        public UserDto? Owner { get; set; }
        public required int GameId { get; set; }
        public required string OwnerId { get; set; }
        public required int Size { get; set; }
        public required int StartRow { get; set; }
        public required int StartColumn { get; set; }
        public required string Direction { get; set; }
        public required bool IsSunk { get; set; }
    }
}
