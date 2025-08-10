using Microsoft.Extensions.Configuration;

namespace InventarioEscolar.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static string ConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("ConnectionSQLServer")!;
        }
    }
}