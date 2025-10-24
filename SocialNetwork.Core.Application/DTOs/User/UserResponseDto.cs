using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.DTOs.User
{
    public class UserResponseDto
    {
        public bool? HasError { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
