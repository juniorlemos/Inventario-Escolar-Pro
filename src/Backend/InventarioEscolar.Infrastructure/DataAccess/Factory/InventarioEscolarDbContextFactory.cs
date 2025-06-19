using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using InventarioEscolar.Infrastructure.Extensions;


namespace InventarioEscolar.Infrastructure.DataAccess.Factory
{
    public class InventarioEscolarDbContextFactory : IDesignTimeDbContextFactory<InventarioEscolarProDBContext>
    {
        public InventarioEscolarProDBContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<InventarioEscolarProDBContext>();
            optionsBuilder.UseSqlServer(configuration.ConnectionString());

            return new InventarioEscolarProDBContext(optionsBuilder.Options);
        }
    }
}