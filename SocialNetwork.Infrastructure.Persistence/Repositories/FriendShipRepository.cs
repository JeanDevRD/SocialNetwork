using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Interfaces;
using SocialNetwork.Infrastructure.Persistence.Context;


namespace SocialNetwork.Infrastructure.Persistence.Repositories
{
    internal class FriendShipRepository : GenericRepository<FriendShip>, IFriendshipRepository
    {
        public FriendShipRepository(SocialNetworkDbContext context) : base(context)
        {
        }
    }
}
