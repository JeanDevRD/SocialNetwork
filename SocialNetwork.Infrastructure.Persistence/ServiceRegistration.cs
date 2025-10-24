using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

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
        }

        
        
    }
}
