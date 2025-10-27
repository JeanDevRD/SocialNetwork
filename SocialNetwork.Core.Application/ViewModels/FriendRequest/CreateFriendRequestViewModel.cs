using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.FriendRequest
{
    public class CreateFriendRequestViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        public string? SelectedUserId { get; set; }
        public List<AvailableUserViewModel> AvailableUsers { get; set; } = new();
    }
}
