using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Interfaces;
using SocialNetwork.Infrastructure.Persistence.Context;

namespace SocialNetwork.Infrastructure.Persistence.Repositories
{
    public class AttackRepository : GenericRepository<Attack>, IAttackRepository
    {
        public AttackRepository(SocialNetworkDbContext context) : base(context)
        {
        }
    }
}
