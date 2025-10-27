using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SocialNetwork.Infrastructure.Core.Application.Interfaces;
using SocialNetwork.Infrastructure.Identity.Context;
using SocialNetwork.Infrastructure.Identity.Entities;
using SocialNetwork.Infrastructure.Identity.Seed;
using SocialNetwork.Infrastructure.Identity.Services;
using System.Reflection;

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

            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            services.AddScoped<IAccountServiceWeb, AccountServiceWeb>();

            ConfigureDbContext(services, configuration);
            #region
            services.AddIdentityCore<UserEntity>()
            .AddSignInManager()
            .AddEntityFrameworkStores<IdentityAppContext>()
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
            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                opt.LoginPath = "/Login/Index";
                opt.LogoutPath = "/Login/Logout";
                opt.AccessDeniedPath = "/Login/AccessDenied";
            });
            #endregion

        }

        public static async Task SeedDefaultUserAsync(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                var userManager = services.GetRequiredService<UserManager<UserEntity>>();
                
                await DefaultUser.SeedAsync(userManager);

            }
        }

        #region private methods
        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<IdentityAppContext>
            (
                (ServiceProvider, Opt) =>
                {
                    
                    Opt.EnableSensitiveDataLogging();
                    Opt.UseSqlServer(connectionString,
                    m => m.MigrationsAssembly(typeof(IdentityAppContext).Assembly.FullName));
                },
                contextLifetime: ServiceLifetime.Scoped,
                optionsLifetime: ServiceLifetime.Scoped

            );
        }
        #endregion
    }
}
