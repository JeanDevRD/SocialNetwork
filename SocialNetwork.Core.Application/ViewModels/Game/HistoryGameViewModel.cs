using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Game
{
    public class HistoryGameViewModel
    {
        public int GameId { get; set; }
        public string? OpponentUsername { get; set; }
        public string? OpponentProfile { get; set; } 
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
        public string? Duration { get; set; } 
        public bool Won { get; set; }
        public string? WinnerUsername { get; set; } 
    }
}
