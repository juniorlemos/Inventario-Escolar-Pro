using InventarioEscolar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess
{
    public class InventárioEscolarProDBContext : DbContext
    {
        public InventárioEscolarProDBContext(DbContextOptions options) : base(options) { }
        
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RoomLocation> RoomLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventárioEscolarProDBContext).Assembly);
        }
    }
}
