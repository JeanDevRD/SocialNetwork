using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Game
{
    public class ActiveGameViewModel
    {
        public int GameId { get; set; }
        public string? OpponentUsername { get; set; } 
        public string? OpponentProfile { get; set; }
        public DateTime Started { get; set; }
        public string? ElapsedTime { get; set; }
    }
}
