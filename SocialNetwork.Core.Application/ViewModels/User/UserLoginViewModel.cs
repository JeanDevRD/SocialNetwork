using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.ViewModels.User
{
    public class UserLoginViewModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
