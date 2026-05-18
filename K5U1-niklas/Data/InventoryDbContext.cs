using K5U1_niklas.Models;
using Microsoft.EntityFrameworkCore;

namespace K5U1_niklas.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
    }
}
