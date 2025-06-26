using InventarioEscolar.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventarioEscolar.Infrastructure.DataAccess
{
    public class InventarioEscolarProDBContext(DbContextOptions<InventarioEscolarProDBContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, long>(options)
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RoomLocation> RoomLocations { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<AssetMovement> AssetMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventarioEscolarProDBContext).Assembly);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (clrType.Namespace?.StartsWith("Microsoft.AspNetCore.Identity") == true)
                    continue;

                var isActiveProp = clrType.GetProperty("Active");
                if (isActiveProp is not null && isActiveProp.PropertyType == typeof(bool))
                {
                    var parameter = Expression.Parameter(clrType, "e");

                    var propertyMethod = typeof(EF)
                        .GetMethods()
                        .First(m => m.Name == "Property" && m.GetParameters().Length == 2)
                        .MakeGenericMethod(typeof(bool));

                    var propertyCall = Expression.Call(
                        propertyMethod,
                        parameter,
                        Expression.Constant("Active"));

                    var comparison = Expression.Equal(propertyCall, Expression.Constant(true));
                    var lambda = Expression.Lambda(comparison, parameter);

                    modelBuilder.Entity(clrType).HasQueryFilter(lambda);

                }
            }
        }
    }
}