using Microsoft.Extensions.Configuration;

namespace InventarioEscolar.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static string ConnectionStringSQLServer(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("ConnectionSQLServer")!;
        }
        public static string ConnectionStringPostgres(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("DefaultConnection")!;
        }
    }
}