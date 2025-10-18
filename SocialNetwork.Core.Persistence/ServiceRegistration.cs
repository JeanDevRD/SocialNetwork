using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Core.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Core.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServicesIoc(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<SocialNetworkDbContext>(opt =>
            opt.UseSqlServer(connectionString,
            m => m.MigrationsAssembly(typeof(SocialNetworkDbContext).Assembly.FullName))
            , ServiceLifetime.Transient);

            services.AddTransient<DbContext, SocialNetworkDbContext>();
        }
    }
}
