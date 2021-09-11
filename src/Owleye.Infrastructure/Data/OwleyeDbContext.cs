using Microsoft.EntityFrameworkCore;
using Owleye.Domain;
using Owleye.Infrastructure.Data.ModelMapping;

namespace Owleye.Infrastructure.Data
{
    public class OwleyeDbContext : DbContext
    {
        public OwleyeDbContext(DbContextOptions options) : base(options)
        {
        }

        public OwleyeDbContext()
        {
        }

        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<EndPoint> EndPoints { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EndPointMapConfiguration());
            modelBuilder.ApplyConfiguration(new SensorMapConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationMapConfiguration());
        }

    }
}
