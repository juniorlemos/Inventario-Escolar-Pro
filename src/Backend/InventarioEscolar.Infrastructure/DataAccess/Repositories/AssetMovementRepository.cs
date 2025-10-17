using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class AssetMovementRepository(InventarioEscolarProDBContext dbContext) : IAssetMovementReadOnlyRepository, IAssetMovementWriteOnlyRepository, IAssetMovementUpdateOnlyRepository
    {
        public async Task Insert(AssetMovement assetMovement) => await dbContext.AssetMovements.AddAsync(assetMovement);
        public void Update(AssetMovement assetMovement) => dbContext.AssetMovements.Update(assetMovement);

        public async Task<PagedResult<AssetMovement>> GetAll(int page, int pageSize,string searchTerm, bool? isCanceled = null)
        {
            var query = dbContext.AssetMovements
                .Include(a => a.Asset)
                .Include(a => a.FromRoom)
                .Include(a => a.ToRoom)
                .AsNoTracking()
                .AsQueryable();

            if (isCanceled.HasValue)
                query = query.Where(a => a.IsCanceled == isCanceled.Value);


            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.ToLower();

                query = query.Where(a =>
                    a.Asset.Name.ToLower().Contains(normalizedSearch) ||
                    a.FromRoom.Name.ToLower().Contains(normalizedSearch) ||
                    a.ToRoom.Name.ToLower().Contains(normalizedSearch) );
            }

            var totalCount = await query.CountAsync();

            var items = new List<AssetMovement>();

            if (page > 0 && pageSize > 0)
            {
                items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                items = await query.ToListAsync();
            }

            return new PagedResult<AssetMovement>(items, totalCount, page, pageSize);
        }
        public async Task<AssetMovement?> GetById(long assetMovementId)
        {
           
         return await dbContext.AssetMovements
        .Include(a => a.Asset)
        .Include(a => a.FromRoom)
        .Include(a => a.ToRoom)
        .FirstOrDefaultAsync(a => a.Id == assetMovementId);
        }
    }
}