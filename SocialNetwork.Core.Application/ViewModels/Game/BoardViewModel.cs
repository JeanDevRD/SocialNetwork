using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Game
{
    public class BoardViewModel
    {
        public int GameId { get; set; }
        public int ShipSize { get; set; }
        public CellViewModel[,] Board { get; set; } = new CellViewModel[12, 12];

        public BoardViewModel()
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Board[i, j] = new CellViewModel
                    {
                        Row = i,
                        Column = j,
                        IsOccupied = false
                    };
                }
            }
        }
    }
}
