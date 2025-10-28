using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Core.Domain.Interfaces;
using SocialNetwork.Infrastructure.Persistence.Context;
using SocialNetwork.Infrastructure.Persistence.Repositories;

namespace SocialNetwork.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServicesIoc(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<SocialNetworkDbContext>
            (
                (ServiceProvider, Opt) =>
                {

                    Opt.EnableSensitiveDataLogging();
                    Opt.UseSqlServer(connectionString,
                    m => m.MigrationsAssembly(typeof(SocialNetworkDbContext).Assembly.FullName));
                },
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Scoped

            );

            services.AddTransient<DbContext, SocialNetworkDbContext>();

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IReactionRepository, ReactionRepository>();
            services.AddTransient<IAttackRepository, AttackRepository>();
            services.AddTransient<IFriendRequestRepository, FriendRequestRepository>();
            services.AddTransient<IFriendshipRepository, FriendShipRepository>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddTransient<IShipRepository, ShipRepository>();
        }

        
        
    }
}
