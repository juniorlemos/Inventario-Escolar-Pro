using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories.ReportsRepository
{
    public sealed class AssetReportRepository(InventarioEscolarProDBContext dbContext) : IAssetReportReadOnlyRepository
    {
        public async Task<IEnumerable<Asset>> GetAllAssetReport()
        {
            return await dbContext.Assets
               .Include(m => m.Category)
               .Include(m => m.RoomLocation)
               .ToListAsync();
        }
    } 
}
