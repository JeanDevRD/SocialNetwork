
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfiguration
{
    public class GameEntityConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Games");
            #endregion

            #region Property configurations
            builder.Property(x => x.Player1Id)
                .IsRequired();

            builder.Property(x => x.Player2Id)
                .IsRequired();

            builder.Property(x => x.Started)
                .IsRequired();

            builder.Property(x => x.Ended)
                .IsRequired(false); 

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20); 

            builder.Property(x => x.WinnerId)
                .IsRequired(false); 

            builder.Property(x => x.CurrentTurnPlayerId)
                .IsRequired();
            #endregion

            #region Relationships

            builder.HasOne(x => x.Winner)
                .WithMany()
                .HasForeignKey(x => x.WinnerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Ships)
                .WithOne(s => s.Game)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Attacks)
                .WithOne(a => a.Game)
                .HasForeignKey(a => a.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Indexes
            builder.HasIndex(x => new { x.Player1Id, x.Status });
            builder.HasIndex(x => new { x.Player2Id, x.Status });
            #endregion
        }
    }
}
