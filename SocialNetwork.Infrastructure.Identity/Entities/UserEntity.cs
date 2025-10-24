using Microsoft.AspNetCore.Identity;

namespace SocialNetwork_Infrastructure.Identity.Entities
{
    public class UserEntity : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Profile { get; set; }
        public required bool IsActive { get; set; }
    }
}
