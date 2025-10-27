using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Core.Application.Interfaces;
using SocialNetwork.Core.Application.Services;
using SocialNetwork.Infrastructure.Core.Application.Interfaces;

using System.Reflection;


namespace SocialNetwork.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServicesIoc(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IReactionService, ReactionService>();
          
            
        }
    }
}
