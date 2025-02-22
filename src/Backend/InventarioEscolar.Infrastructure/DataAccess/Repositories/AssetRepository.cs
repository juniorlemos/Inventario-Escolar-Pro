using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories.Asset;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public class AssetRepository : IAssetReadOnlyRepository , IAssetWriteOnlyRepository
    {
        private readonly InventárioEscolarProDBContext _dbContext;
        public AssetRepository(InventárioEscolarProDBContext dbContext) => _dbContext = dbContext;

        public async Task Add(Asset asset) => await _dbContext.Assets.AddAsync(asset);

        public async Task<IList<Asset>> GetAllAssets()
        {
            return await _dbContext.Assets.AsNoTracking()
                .Include( c => c.Category)
                .Include( l => l.RoomLocation )
                .ToListAsync();
        }

        public Task<Asset?> GetById(Asset user, long assetId)
        {
            throw new NotImplementedException();
        }
    }
}
