using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories.Asset;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public class AssetRepository : IAssetReadOnlyRepository , IAssetWriteOnlyRepository, IAssetUpdateOnlyRepository
    {
        private readonly InventárioEscolarProDBContext _dbContext;
        public AssetRepository(InventárioEscolarProDBContext dbContext) => _dbContext = dbContext;

        public async Task Add(Asset asset) => await _dbContext.Assets.AddAsync(asset);

        public async Task Delete(long assetId)
        {
            var asset = await _dbContext.Assets.FindAsync(assetId);

            _dbContext.Assets.Remove(asset!);
        }

        public async Task<IEnumerable<Asset>> GetAllAssets()
        {
            return await _dbContext.Assets.AsNoTracking()
                .Include( c => c.Category)
                .Include( l => l.RoomLocation )
                .ToListAsync();
        }
        public async Task<Asset?> GetById(long assetId)
        {
            return await _dbContext.Assets.AsNoTracking()
                                          .Include(c => c.Category)
                                          .Include(l => l.RoomLocation)
                                          .FirstOrDefaultAsync(a => a.Id == assetId);           
        }
        public void Update(Asset user) => _dbContext.Assets.Update(user);
    }
}
