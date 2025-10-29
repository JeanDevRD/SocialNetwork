using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Game
{
    public class SelectDirectionViewModel
    {
        public int GameId { get; set; }
        public int ShipSize { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una dirección")]
        public string? Direction { get; set; }
    }
}
