using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Identity.Context
{
    public class IdentityAppContext : IdentityDbContext<UserEntity>
    {
        public IdentityAppContext(DbContextOptions<IdentityAppContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");

            builder.Entity<UserEntity>().ToTable("Users");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
           


        }
    }
}
