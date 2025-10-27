using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.FriendRequest
{
    public class ConfirmActionViewModel
    {
        public int Id { get; set; }
        public string? Action { get; set; }
        public string? Message { get; set; }
    }
}
