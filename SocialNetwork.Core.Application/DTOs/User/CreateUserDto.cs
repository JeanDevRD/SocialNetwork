using SocialNetwork.Core.Application.DTOs.CommonEntity;


namespace SocialNetwork.Core.Application.DTOs.User
{
    public class CreateUserDto 
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string PasswordHash { get; set; }
        public required string Profile { get; set; }
        public required bool IsActive { get; set; }
    }
}
