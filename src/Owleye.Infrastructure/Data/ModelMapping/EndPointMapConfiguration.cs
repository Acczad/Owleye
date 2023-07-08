using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Owleye.Domain;

namespace Owleye.Infrastructure.Data.ModelMapping
{
    public class EndPointMapConfiguration : IEntityTypeConfiguration<EndPoint>
    {
        public void Configure(EntityTypeBuilder<EndPoint> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(t => t.Name)
                .IsRequired(true)
                .HasMaxLength(255);

            builder.Property(t => t.Url)
                .HasMaxLength(500);

            builder.Property(t => t.IpAddress)
                .HasMaxLength(21);

            builder.Property(t => t.WebPageMetaKeyword)
                .HasMaxLength(255);
        }
    }
}
