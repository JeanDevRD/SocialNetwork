using SocialNetwork.Core.Application.DTOs.User;

namespace SocialNetwork.Core.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> AddAsync(CreateUserDto dto);
        Task<UserDto?> UpdateAsync(CreateUserDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetById(int id);
        Task<UserDto?> LoginAsync(UserLoginDto dto);
        Task<bool> ChangeStatusAsync(int id);
    }
}
