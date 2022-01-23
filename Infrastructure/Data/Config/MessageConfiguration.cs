using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(p => p.TextMessage).IsRequired().HasMaxLength(300);
            builder.Property(p => p.TimeOfMessage).IsRequired();
            builder.HasOne(p => p.SentByUser).WithMany().HasForeignKey(p => p.SentByUserId);
            builder.HasOne(p => p.SentToUser).WithMany().HasForeignKey(p => p.SentToUserId);
        }
    }
}