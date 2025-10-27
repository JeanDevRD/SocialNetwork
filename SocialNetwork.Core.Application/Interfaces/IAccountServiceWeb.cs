using SocialNetwork.Core.Application.DTOs.User;

namespace SocialNetwork.Infrastructure.Core.Application.Interfaces
{
    public interface IAccountServiceWeb
    {
        Task<LoginResponseDto> AuthenticateAsync(UserLoginDto dto);
        Task<string> ConfirmationAccountAsync(string userId, string token);
        Task<UserResponseDto> DeleteAsync(string userId);
        Task<EditResponseDto> EditUser(CreateUserDto dto, string origin);
        Task<UserResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
        Task<List<UserDto>> GetAllUser(bool? isActive = true);
        Task<UserDto> GetUserByEmail(string Email);
        Task<UserDto> GetUserByUserName(string UserName);
        Task LogoutAsync();
        Task<RegisterResponseDto> RegisterUser(CreateUserDto dto, string origin);
        Task<UserResponseDto> ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}