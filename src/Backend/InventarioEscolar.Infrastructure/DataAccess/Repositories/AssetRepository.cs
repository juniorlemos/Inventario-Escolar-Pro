using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Repositories.Assets;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public class AssetRepository(InventarioEscolarProDBContext dbContext) : IAssetReadOnlyRepository, IAssetWriteOnlyRepository, IAssetUpdateOnlyRepository, IAssetDeleteOnlyRepository
    {
        public async Task Insert(Asset asset) => await dbContext.Assets.AddAsync(asset);

        public async Task<bool> ExistPatrimonyCode(long? patromonyCode)
        {
            return await dbContext.Assets.AnyAsync(p => p.PatrimonyCode.Equals(patromonyCode));
        }

        public async Task<PagedResult<Asset>> GetAll(int page, int pageSize)
        {
            var query = dbContext.Assets.AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = new List<Asset>();

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

            return new PagedResult<Asset>(items, totalCount, page, pageSize);
        }

        public async Task<Asset?> GetById(long assetId)
        {

            return await dbContext.Assets.FirstOrDefaultAsync(asset => asset.Id == assetId);
        }
        public void Update(Asset asset) => dbContext.Assets.Update(asset);
      
        public async Task<bool> Delete(long assetId)
        {
            var assets = await dbContext.Assets.FindAsync(assetId);

            if (assets == null)
                return false;

            dbContext.Assets.Remove(assets);
            return true;
        }
    }
}
