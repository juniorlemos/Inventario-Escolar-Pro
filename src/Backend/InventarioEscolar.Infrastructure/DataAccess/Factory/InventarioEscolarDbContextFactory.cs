using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


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

            var fakeCurrentUserService = new FakeCurrentUserService();

            return new InventarioEscolarProDBContext(optionsBuilder.Options, fakeCurrentUserService);
        }

        private class FakeCurrentUserService : ICurrentUserService
        {
            public long SchoolId => -1;
            public long? UserId => null;
            public string UserName => string.Empty;
            public bool IsAuthenticated => true;
        }
    }
}