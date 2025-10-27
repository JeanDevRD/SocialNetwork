using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Home
{
    public class ReactionDetailViewModel
    {
        public int Id { get; set; }
        public string? Type { get; set; } 
        public string? UserId { get; set; } 
    }
}
