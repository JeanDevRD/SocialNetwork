using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SocialNetwork_Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace SocialNetwork.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityServicesIocWeb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;


            });

            ConfigureDbContext(services, configuration);
            #region
            services.AddIdentityCore<UserEntity>()
            .AddSignInManager()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<UserEntity>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(2);
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                opt.LoginPath = "/Login/Index";
                opt.LogoutPath = "/Login/Logout";
                opt.AccessDeniedPath = "/Login/AccessDenied";
            });
            #endregion

        }

        #region private methods
        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("IdentityConnection");

            services.AddDbContext<IdentityDbContext>
            (
                (ServiceProvider, Opt) =>
                {
                    
                    Opt.EnableSensitiveDataLogging();
                    Opt.UseSqlServer(connectionString,
                    m => m.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName));
                },
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Scoped

            );
        }
        #endregion
    }
}
