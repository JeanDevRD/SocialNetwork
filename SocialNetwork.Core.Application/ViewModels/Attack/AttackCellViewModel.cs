using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Attack
{
    public class AttackCellViewModel
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public AttackStatus Status { get; set; }
    }
}
