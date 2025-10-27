using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.FriendShip
{
    public class DeleteFriendViewModel
    {
        public int FriendShipId { get; set; }
        public string FriendName { get; set; } = string.Empty;
    }
}
