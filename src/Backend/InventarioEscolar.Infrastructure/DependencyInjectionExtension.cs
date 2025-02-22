using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Asset;
using InventarioEscolar.Infrastructure.DataAccess;
using InventarioEscolar.Infrastructure.DataAccess.Repositories;
using InventarioEscolar.Infrastructure.Extensions;
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
        }
        private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddDbContext<InventárioEscolarProDBContext>(dbContextOptions =>
            {
                dbContextOptions.UseSqlServer(connectionString);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAssetWriteOnlyRepository, AssetRepository>();
            services.AddScoped<IAssetReadOnlyRepository, AssetRepository>();
        }
    }
}
