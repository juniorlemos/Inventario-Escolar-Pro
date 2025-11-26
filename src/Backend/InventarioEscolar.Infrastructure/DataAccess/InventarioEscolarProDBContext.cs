using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventarioEscolar.Infrastructure.DataAccess
{
    public class InventarioEscolarProDBContext
        : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        private readonly ICurrentUserService _currentUserService;

        public InventarioEscolarProDBContext(
            DbContextOptions<InventarioEscolarProDBContext> options,
            ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RoomLocation> RoomLocations { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<AssetMovement> AssetMovements { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(InventarioEscolarProDBContext).Assembly);

            var schoolEntityType = typeof(ISchoolEntity);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (clrType.Namespace?.StartsWith("Microsoft.AspNetCore.Identity") == true)
                    continue;

                var parameter = Expression.Parameter(clrType, "e");
                Expression? filter = null;

                // FILTRO Active == true
                var activeProp = clrType.GetProperty("Active");
                if (activeProp is not null && activeProp.PropertyType == typeof(bool))
                {
                    var activeAttr = Expression.Call(
                        typeof(EF).GetMethod("Property")!.MakeGenericMethod(typeof(bool)),
                        parameter,
                        Expression.Constant("Active"));

                    var activeFilter = Expression.Equal(activeAttr, Expression.Constant(true));
                    filter = activeFilter;
                }

                // FILTRO SchoolId
                if (schoolEntityType.IsAssignableFrom(clrType))
                {
                    var schoolIdAttr = Expression.Call(
                        typeof(EF).GetMethod("Property")!.MakeGenericMethod(typeof(long)),
                        parameter,
                        Expression.Constant("SchoolId"));

                    var currentSchoolId = Expression.Constant(_currentUserService.SchoolId);

                    var schoolFilter = Expression.Equal(schoolIdAttr, currentSchoolId);

                    filter = filter is not null
                        ? Expression.AndAlso(filter, schoolFilter)
                        : schoolFilter;
                }

                if (filter != null)
                {
                    var lambda = Expression.Lambda(filter, parameter);
                    modelBuilder.Entity(clrType).HasQueryFilter(lambda);
                }
            }

            // FILTRO DO APPLICATIONUSER
            modelBuilder.Entity<ApplicationUser>()
                .HasQueryFilter(u =>
                    !_currentUserService.IsAuthenticated ||
                    u.SchoolId == _currentUserService.SchoolId
                );
        }
    }
}
