using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.DTOs.User
{
    public class RegisterResponseDto
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Profile { get; set; } = "Images/DefaultProfile.png";
        public bool IsVerified { get; set; }
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
