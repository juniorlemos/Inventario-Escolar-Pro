using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventarioEscolar.Infrastructure.DataAccess
{
    public class InventarioEscolarProDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventarioEscolarProDBContext).Assembly);

            var schoolEntityType = typeof(ISchoolEntity);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (clrType.Namespace?.StartsWith("Microsoft.AspNetCore.Identity") == true)
                    continue;

                var parameter = Expression.Parameter(clrType, "e");
                Expression? filter = null;

                // Filtro: e => EF.Property<bool>(e, "Active") == true
                var isActiveProp = clrType.GetProperty("Active");
                if (isActiveProp is not null && isActiveProp.PropertyType == typeof(bool))
                {
                    var activeProperty = Expression.Call(
                        typeof(EF).GetMethod("Property")!.MakeGenericMethod(typeof(bool)),
                        parameter,
                        Expression.Constant("Active"));

                    var activeComparison = Expression.Equal(activeProperty, Expression.Constant(true));
                    filter = activeComparison;
                }

                // Filtro de SchoolId
                if (schoolEntityType.IsAssignableFrom(clrType))
                {
                    var schoolIdProperty = Expression.Call(
                        typeof(EF).GetMethod("Property")!.MakeGenericMethod(typeof(long)),
                        parameter,
                        Expression.Constant("SchoolId"));

                    var currentSchoolId = Expression.Property(
                        Expression.Constant(_currentUserService),
                        nameof(ICurrentUserService.SchoolId)
                    );

                    var hasValue = Expression.Property(currentSchoolId, "HasValue");
                    var schoolIdValue = Expression.Property(currentSchoolId, "Value");

                    var schoolComparison = Expression.Equal(schoolIdProperty, schoolIdValue);
                    var schoolFilter = Expression.OrElse(Expression.IsFalse(hasValue), schoolComparison);

                    filter = filter is not null
                        ? Expression.AndAlso(filter, schoolFilter)
                        : schoolFilter;
                }

                // Se tiver algum filtro, aplica
                if (filter is not null)
                {
                    var lambda = Expression.Lambda(filter, parameter);
                    modelBuilder.Entity(clrType).HasQueryFilter(lambda);
                }
            }
        }
    }
}