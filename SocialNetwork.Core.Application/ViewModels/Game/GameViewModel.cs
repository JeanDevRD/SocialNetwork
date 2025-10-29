using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Game
{
    public class GameViewModel
    {
        public List<ActiveGameViewModel> ActiveGames { get; set; } = new();
        public List<HistoryGameViewModel> HistoryGames { get; set; } = new();
        public GameStatsViewModel Stats { get; set; } = new();
    }
}
