using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfiguration
{
    public class AttackEntityConfiguration : IEntityTypeConfiguration<Attack>
    {
        public void Configure(EntityTypeBuilder<Attack> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Attacks");
            #endregion

            #region Property configurations
            builder.Property(x => x.GameId)
                .IsRequired();

            builder.Property(x => x.AttackerId)
                .IsRequired();

            builder.Property(x => x.Row)
                .IsRequired();

            builder.Property(x => x.Column)
                .IsRequired();

            builder.Property(x => x.Hit)
                .IsRequired();

            builder.Property(x => x.Attacked)
                .IsRequired();
            #endregion

            #region Relationships

            builder.HasOne(x => x.Attacker)
                .WithMany()
                .HasForeignKey(x => x.AttackerId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Indexes
          
            builder.HasIndex(x => x.GameId);
            builder.HasIndex(x => new { x.GameId, x.Row, x.Column })
                .IsUnique();
            #endregion
        }
    }
}
