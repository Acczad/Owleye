using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Owleye.Domain;

namespace Owleye.Infrastructure.Data.ModelMapping
{
    public class SensorMapConfiguration:IEntityTypeConfiguration<Sensor>
    {
        public void Configure(EntityTypeBuilder<Sensor> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired(true)
                .HasMaxLength(255);
        }
    }
}
