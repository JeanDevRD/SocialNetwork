using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Domain.Entities;
using System.Reflection;

namespace SocialNetwork.Core.Persistence.Context
{
    public class SocialNetworkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<FriendShip> FriendShips { get; set; }
        public DbSet<Attack> Attacks { get; set; }

        public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}