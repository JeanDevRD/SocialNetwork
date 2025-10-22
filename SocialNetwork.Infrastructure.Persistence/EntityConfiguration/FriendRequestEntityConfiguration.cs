using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Domain.Entities;


namespace SocialNetwork.Infrastructure.Persistence.EntityConfiguration
{
    public class FriendRequestEntityConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("FriendRequests");
            #endregion

            #region Property configurations
            builder.Property(x => x.SenderId)
                .IsRequired();

            builder.Property(x => x.ReceiverId)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20); 

            builder.Property(x => x.RequestedAt)
                .IsRequired();
            #endregion

            #region Indexes
     
            builder.HasIndex(x => new { x.ReceiverId, x.Status });

            builder.HasIndex(x => new { x.SenderId, x.Status });
            #endregion


        }
    }
}
