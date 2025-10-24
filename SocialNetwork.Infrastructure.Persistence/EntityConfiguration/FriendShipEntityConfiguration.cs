using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfiguration
{
    public class FriendShipEntityConfiguration : IEntityTypeConfiguration<FriendShip>
    {
        public void Configure(EntityTypeBuilder<FriendShip> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("FriendShips");
            #endregion

            #region Property configurations
            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.FriendId)
                .IsRequired();

            builder.Property(x => x.Created)
                .IsRequired();
            #endregion

            #region Relationships
           
            #endregion

            #region Indexes

            builder.HasIndex(x => new { x.UserId, x.FriendId })
                .IsUnique();
            #endregion
        }
    }
}
