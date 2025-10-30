using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Game
{
    public class SurrenderViewModel
    {
        public int GameId { get; set; }
        public string? OpponentUsername { get; set; } 
    }
}
