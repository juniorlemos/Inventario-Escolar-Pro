using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories.ReportsRepository
{
    public sealed class AssetMovementReportRepository(InventarioEscolarProDBContext dbContext) : IAssetMovementReportReadOnlyRepository
    {
        public async Task<IEnumerable<AssetMovement>> GetAllAssetMovementsReport()
        {
            return await dbContext.AssetMovements
               .Include(m => m.Asset)
               .Include(m => m.FromRoom)
               .Include(m => m.ToRoom)
               .OrderByDescending(m => m.CanceledAt)
               .ToListAsync();
        }
        public async Task<IEnumerable<AssetMovement>> GetAllAssetCanceledMovementsReport()

        {
            return await dbContext.AssetMovements
                          .Include(m => m.Asset)
                          .Include(m => m.FromRoom)
                          .Include(m => m.ToRoom)
                          .Where(m => m.IsCanceled)
                          .OrderByDescending(m => m.CanceledAt)
                          .ToListAsync();
        }
    }
}
