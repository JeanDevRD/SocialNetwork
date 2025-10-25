
namespace SocialNetwork.Core.Application.DTOs.User
{
    public class EditResponseDto
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Profile { get; set; } = "Images/DefaultProfile.png";
        public bool IsVerified { get; set; }
        public bool HasError { get; set; }
        public List<string>? ErrorMessage { get; set; }
    }
}
