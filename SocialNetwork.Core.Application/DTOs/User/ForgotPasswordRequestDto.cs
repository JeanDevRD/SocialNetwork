using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.DTOs.User
{
    public class ForgotPasswordRequestDto
    {
        public required string UserName { get; set; }
        public required string Origin { get; set; }
    }
}
