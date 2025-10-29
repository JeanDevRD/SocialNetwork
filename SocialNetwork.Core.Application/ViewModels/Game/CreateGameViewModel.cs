using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Game
{
    public class CreateGameViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un amigo para jugar")]
        public string? SelectedFriendId { get; set; }

        public List<AvailableFriendViewModel> AvailableFriends { get; set; } = new();
    }
}
