using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Game
{
    public class CellViewModel
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsOccupied { get; set; }
    }
}
