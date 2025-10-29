using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Ship
{
    public class SelectShipViewModel
    {
        public int GameId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un barco")]
        public int? SelectedShipSize { get; set; }

        public List<ShipOptionViewModel> AvailableShips { get; set; } = new();
        public bool OpponentReady { get; set; }
    }
}
