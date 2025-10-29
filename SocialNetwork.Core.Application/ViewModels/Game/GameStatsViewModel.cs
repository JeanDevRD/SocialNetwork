using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Game
{
    public class GameStatsViewModel
    {
        public int? TotalGames { get; set; }
        public int? GamesWon { get; set; }
        public int? GamesLost { get; set; }
    }
}
