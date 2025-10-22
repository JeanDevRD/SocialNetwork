using SocialNetwork.Core.Application.DTOs.CommonEntity;


namespace SocialNetwork.Core.Application.DTOs.User
{
    public class CreateUserDto : CommonEntityDto<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Phone { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Profile { get; set; }
        public required bool IsActive { get; set; }
    }
}
