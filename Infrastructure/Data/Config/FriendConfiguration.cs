using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class FriendConfiguration : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId);
            builder.HasOne(p => p.FriendUser).WithMany().HasForeignKey(p => p.FriendUserId);
        }
    }
}