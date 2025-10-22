using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Core.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email,string password);
    }
}
