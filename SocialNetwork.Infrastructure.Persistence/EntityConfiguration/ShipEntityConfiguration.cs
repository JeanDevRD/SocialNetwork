using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Domain.Entities;


namespace SocialNetwork.Infrastructure.Persistence.EntityConfiguration
{
    public class ShipEntityConfiguration : IEntityTypeConfiguration<Ship>
    {
        public void Configure(EntityTypeBuilder<Ship> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Ships");
            #endregion

            #region Property configurations
            builder.Property(x => x.GameId)
                .IsRequired();

            builder.Property(x => x.OwnerId)
                .IsRequired();

            builder.Property(x => x.Size)
                .IsRequired();

            builder.Property(x => x.StartRow)
                .IsRequired();

            builder.Property(x => x.StartColumn)
                .IsRequired();

            builder.Property(x => x.Direction)
                .IsRequired()
                .HasMaxLength(10); 

            builder.Property(x => x.IsSunk)
                .IsRequired()
                .HasDefaultValue(false);
            #endregion

            #region Relationships
            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Indexes
            builder.HasIndex(x => new { x.GameId, x.OwnerId });
            #endregion
        }
    }
}
