using InventarioEscolar.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess
{
    public class InventarioEscolarProDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public InventarioEscolarProDBContext(DbContextOptions<InventarioEscolarProDBContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RoomLocation> RoomLocations { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<AssetMovement> AssetMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventarioEscolarProDBContext).Assembly);
        }
    }
}