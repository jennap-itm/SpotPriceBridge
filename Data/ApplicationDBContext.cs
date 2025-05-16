using Microsoft.EntityFrameworkCore;
using SpotPriceBridge.Models;

namespace SpotPriceBridge.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SpotPriceModel> NewSpotPrice { get; set; }
    }
}
