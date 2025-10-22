using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Interfaces;
using SocialNetwork.Infrastructure.Persistence.Context;

namespace SocialNetwork.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User> , IUserRepository
    { 
        public UserRepository(SocialNetworkDbContext context) : base(context)
        { 

        }
        public async Task<User?> GetByEmailAsync(string email, string password)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            User? result = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            if (result == null)
            {
                return null;
            }
                return result;
        }
    }
}
