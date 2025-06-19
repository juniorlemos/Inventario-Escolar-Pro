using Azure;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using InventarioEscolar.Domain.Repositories.Assets;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public class AssetRepository : IAssetReadOnlyRepository, IAssetWriteOnlyRepository, IAssetUpdateOnlyRepository
    {
        private readonly InventarioEscolarProDBContext _dbContext;
        public AssetRepository(InventarioEscolarProDBContext dbContext) => _dbContext = dbContext;

        public async Task Insert(Asset asset) => await _dbContext.Assets.AddAsync(asset);

        public async Task Delete(long assetId)
        {
            var asset = await _dbContext.Assets.FindAsync(assetId);

            _dbContext.Assets.Remove(asset!);
        }

        public async Task<bool> ExistPatrimonyCode(long? patromonyCode)
        {
            return await _dbContext.Assets.AnyAsync(p => p.PatrimonyCode.Equals(patromonyCode));
        }

        public async Task<PagedResult<Asset>> GetAllAssets(int page, int pageSize)
        {
            var query = _dbContext.Assets
                .AsNoTracking()
                 .Select(a => new Asset
                 {
                     Id = a.Id,
                     Name = a.Name,
                     Description = a.Description,
                     PatrimonyCode = a.PatrimonyCode,
                     AcquisitionValue = a.AcquisitionValue,
                     ConservationState = a.ConservationState,
                     SerieNumber = a.SerieNumber,
                     Category = new Category
                     {
                         Id = a.Category.Id,
                         Name = a.Category.Name,
                         Description = a.Category.Description
                     },
                     RoomLocation = new RoomLocation
                     {
                         Id = a.RoomLocation.Id,
                         Name = a.RoomLocation.Name,
                         Description = a.RoomLocation.Description,
                         Building = a.RoomLocation.Building
                     }
                 });

            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<Asset>(items, totalCount, page, pageSize);
        }
        public async Task<Asset?> GetById(long assetId)
        {

            var asset = await _dbContext.Assets.AsNoTracking()
        .Where(a => a.Id == assetId)
        .Select(a => new Asset
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            PatrimonyCode = a.PatrimonyCode,
            AcquisitionValue = a.AcquisitionValue,
            ConservationState = a.ConservationState,
            SerieNumber = a.SerieNumber,
            Category = new Category
            {
                Id = a.Category.Id,
                Name = a.Category.Name,
                Description = a.Category.Description
            },
            RoomLocation = new RoomLocation
            {
                Id = a.RoomLocation.Id,
                Name = a.RoomLocation.Name,
                Description = a.RoomLocation.Description,
                Building = a.RoomLocation.Building
            }
        })
        .FirstOrDefaultAsync();

            return asset;
        }
        public void Update(Asset user) => _dbContext.Assets.Update(user);

    }
}
