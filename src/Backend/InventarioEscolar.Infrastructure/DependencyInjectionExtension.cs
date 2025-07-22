using InventarioEscolar.Application.Services.Interfaces;
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
using InventarioEscolar.Exceptions.ExceptionsBase.Identity;
using InventarioEscolar.Infrastructure.DataAccess;
using InventarioEscolar.Infrastructure.DataAccess.Repositories;
using InventarioEscolar.Infrastructure.Extensions;
using InventarioEscolar.Infrastructure.Reports.AssetByCategoryCase;
using InventarioEscolar.Infrastructure.Reports.AssetByLocationCase;
using InventarioEscolar.Infrastructure.Reports.AssetConservation;
using InventarioEscolar.Infrastructure.Reports.AssetMovementsCase;
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
            AddDbContext_SqlServer(services, configuration);
            AddRepositories(services);
            AddIdentity(services);
            addReports(services);
            AddServices(services);

        }

        private static void AddIdentity( IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;


            })
                    .AddEntityFrameworkStores<InventarioEscolarProDBContext>()
                    .AddErrorDescriber<PortugueseIdentityErrorDescriber>()
                    .AddDefaultTokenProviders();
        }
        
        private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddDbContext<InventarioEscolarProDBContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
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

        }

        private static void addReports( IServiceCollection services)
        {
            services.AddScoped<IInventoryReportGenerator, InventoryReportGenerator>();
            services.AddScoped<IAssetConservationStateReportGenerator, AssetConservationStateReportGenerator>();
            services.AddScoped<IAssetByLocationReportGenerator, AssetByLocationReportGenerator>();
            services.AddScoped<IAssetMovementsReportGenerator, AssetMovementsReportGenerator>();
            services.AddScoped<IAssetByCategoryReportGenerator, AssetByCategoryReportGenerator>();
        }
        public static void AddServices( IServiceCollection services)
        {
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
        }
       
    }
}
