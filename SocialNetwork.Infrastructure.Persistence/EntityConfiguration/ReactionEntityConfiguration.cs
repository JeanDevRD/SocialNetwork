using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Domain.Entities;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfiguration
{
    public class ReactionEntityConfiguration : IEntityTypeConfiguration<Reaction> 
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Reactions");
            #endregion

            #region Property configurations
            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(20); 

            builder.Property(x => x.Created)
                .IsRequired();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.PostId)
                .IsRequired();
            #endregion

            #region Relationships
            #endregion

            #region Indexes
            builder.HasIndex(x => new { x.UserId, x.PostId })
                .IsUnique();
            #endregion
        }
    }
}
    


    

