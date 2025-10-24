using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.DTOs.User
{
    public class ResetPasswordRequestDto
    {
        public required string Id { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }
    }
}
