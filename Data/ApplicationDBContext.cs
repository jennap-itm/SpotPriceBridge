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

        public DbSet<SpotPriceModel> SpotPrice { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SpotPriceModel>()
                .ToTable("NewSpotPrice");
        }
    }
}
