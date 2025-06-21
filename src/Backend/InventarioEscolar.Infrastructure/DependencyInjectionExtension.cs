using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Assets;
using InventarioEscolar.Domain.Repositories.Categories;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using InventarioEscolar.Domain.Repositories.Schools;
using InventarioEscolar.Exceptions.ExceptionsBase.Identity;
using InventarioEscolar.Infrastructure.DataAccess;
using InventarioEscolar.Infrastructure.DataAccess.Repositories;
using InventarioEscolar.Infrastructure.Extensions;
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
    }
}
