using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Ship
{
    public class PlaceShipViewModel
    {
        public int GameId { get; set; }
        public int ShipSize { get; set; }
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public string? Direction { get; set; }
    }
}
