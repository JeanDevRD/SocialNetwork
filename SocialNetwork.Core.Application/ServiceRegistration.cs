using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServicesIoc(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        }
    }
}
