using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Owleye.Domain;

namespace Owleye.Infrastructure.Data.ModelMapping
{
    public class NotificationMapConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.NoTificationAddress).HasMaxLength(2048);
            builder.Property(t => t.NotificationType);
        }
    }
}
