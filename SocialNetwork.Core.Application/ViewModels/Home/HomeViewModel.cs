using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<PostDetailViewModel> Posts { get; set; } = new();
    }
}
