using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Attack
{
    public class AttackBoardViewModel
    {
        public int GameId { get; set; }
        public bool IsMyTurn { get; set; }
        public string OpponentUsername { get; set; } = string.Empty;
        public AttackCellViewModel[,] Board { get; set; } = new AttackCellViewModel[12, 12];

        public AttackBoardViewModel()
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Board[i, j] = new AttackCellViewModel
                    {
                        Row = i,
                        Column = j,
                        Status = AttackStatus.NotAttacked
                    };
                }
            }
        }
    }
}

