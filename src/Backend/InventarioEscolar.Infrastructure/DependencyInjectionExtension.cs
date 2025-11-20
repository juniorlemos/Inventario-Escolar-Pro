using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase;
using InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using InventarioEscolar.Exceptions.ExceptionsBase.Identity;
using InventarioEscolar.Infrastructure.DataAccess;
using InventarioEscolar.Infrastructure.DataAccess.Repositories;
using InventarioEscolar.Infrastructure.DataAccess.Repositories.ReportsRepository;
using InventarioEscolar.Infrastructure.Extensions;
using InventarioEscolar.Infrastructure.Reports.AssetByCategoryCase;
using InventarioEscolar.Infrastructure.Reports.AssetByLocation;
using InventarioEscolar.Infrastructure.Reports.AssetCanceledMovements;
using InventarioEscolar.Infrastructure.Reports.AssetConservation;
using InventarioEscolar.Infrastructure.Reports.AssetMovementsCase;
using InventarioEscolar.Infrastructure.Reports.Inventory;
using InventarioEscolar.Infrastructure.Security.Token;
using InventarioEscolar.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventarioEscolar.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext_PostgreSql(services, configuration);
            AddRepositories(services);
            AddIdentity(services);
            AddReports(services);
            AddServices(services);
        }

        private static void AddIdentity( IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
            })
                    .AddEntityFrameworkStores<InventarioEscolarProDBContext>()
                    .AddErrorDescriber<PortugueseIdentityErrorDescriber>()
                    .AddDefaultTokenProviders();
        }
        
        private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionStringSQLServer();

            services.AddDbContext<InventarioEscolarProDBContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }

        private static void AddDbContext_PostgreSql(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionStringPostgres();

            services.AddDbContext<InventarioEscolarProDBContext>(dbContextOptions =>
            {
                dbContextOptions.UseNpgsql(connectionString);
            });
        }
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAssetWriteOnlyRepository, AssetRepository>();
            services.AddScoped<IAssetReadOnlyRepository, AssetRepository>();
            services.AddScoped<IAssetUpdateOnlyRepository, AssetRepository>();
            services.AddScoped<IAssetDeleteOnlyRepository, AssetRepository>();

            services.AddScoped<IAssetMovementWriteOnlyRepository, AssetMovementRepository>();
            services.AddScoped<IAssetMovementReadOnlyRepository, AssetMovementRepository>();
            services.AddScoped<IAssetMovementUpdateOnlyRepository, AssetMovementRepository>();

            services.AddScoped<IRoomLocationWriteOnlyRepository, RoomLocationRepository>();
            services.AddScoped<IRoomLocationReadOnlyRepository, RoomLocationRepository>();
            services.AddScoped<IRoomLocationUpdateOnlyRepository, RoomLocationRepository>();
            services.AddScoped<IRoomLocationDeleteOnlyRepository, RoomLocationRepository>();

            services.AddScoped<ISchoolWriteOnlyRepository, SchoolRepository>();
            services.AddScoped<ISchoolReadOnlyRepository, SchoolRepository>();
            services.AddScoped<ISchoolUpdateOnlyRepository, SchoolRepository>();
            services.AddScoped<ISchoolDeleteOnlyRepository, SchoolRepository>();

            services.AddScoped<ICategoryWriteOnlyRepository, CategoryRepository>();
            services.AddScoped<ICategoryReadOnlyRepository, CategoryRepository>();
            services.AddScoped<ICategoryUpdateOnlyRepository, CategoryRepository>();
            services.AddScoped<ICategoryDeleteOnlyRepository, CategoryRepository>();

            services.AddScoped<IAssetMovementReportReadOnlyRepository, AssetMovementReportRepository>();
            services.AddScoped<IAssetReportReadOnlyRepository, AssetReportRepository>();
        }

        private static void AddReports( IServiceCollection services)
        {
            services.AddScoped<IInventoryReportGenerator, InventoryReportGenerator>();
            services.AddScoped<IAssetConservationStateReportGenerator, AssetConservationStateReportGenerator>();
            services.AddScoped<IAssetByLocationReportGenerator, AssetByLocationReportGenerator>();
            services.AddScoped<IAssetMovementsReportGenerator, AssetMovementsReportGenerator>();
            services.AddScoped<IAssetByCategoryReportGenerator, AssetByCategoryReportGenerator>();
            services.AddScoped<IAssetCanceledMovementsReportGenerator, AssetCanceledMovementsReportGenerator>();
        }
        public static void AddServices( IServiceCollection services)
        {
            services.AddScoped<ITokenService, JwtTokenGenerator>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
        }
    }
}