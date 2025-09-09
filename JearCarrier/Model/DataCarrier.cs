using Microsoft.EntityFrameworkCore;

namespace JearCarrier.Model
{
    public class DataCarrier : DbContext
    {
        public DataCarrier(DbContextOptions<DataCarrier> options) : base(options) { }

        public DbSet<Carrier> Carriers { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Carrier>().ToTable("Carriers");
            b.Entity<Carrier>().HasIndex(x => x.CarrierName);
            b.Entity<Carrier>().HasIndex(x => x.City);
            b.Entity<Carrier>().HasIndex(x => x.State);
        }
    }
}
