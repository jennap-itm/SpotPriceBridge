using Microsoft.EntityFrameworkCore;
using SpotPriceBridge.Models;

namespace SpotPriceBridge.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SpotPriceModel> NewSpotPrice { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpotPriceModel>().ToTable("NewSpotPrice");

            modelBuilder.Entity<SpotPriceModel>()
                .Property(p => p.AskPrice)
                .HasColumnType("decimal(18,4)"); 

            base.OnModelCreating(modelBuilder);
        }

    }
}
