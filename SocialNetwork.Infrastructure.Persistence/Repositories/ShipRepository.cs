using SocialNetwork.Core.Domain.Entities;
using SocialNetwork.Core.Domain.Interfaces;
using SocialNetwork.Infrastructure.Persistence.Context;


namespace SocialNetwork.Infrastructure.Persistence.Repositories
{
    public class ShipRepository : GenericRepository<Ship>, IShipRepository
    {
        public ShipRepository(SocialNetworkDbContext context) : base(context)
        {
        }
    }
}
