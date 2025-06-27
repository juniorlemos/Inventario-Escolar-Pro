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
            var query = dbContext.Assets
                .Select(a => new Asset
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    PatrimonyCode = a.PatrimonyCode,
                    AcquisitionValue = a.AcquisitionValue,
                    ConservationState = a.ConservationState,
                    SerieNumber = a.SerieNumber,
                    RoomLocation = new RoomLocation
                    {
                        Id = a.Category.Id,
                        Name = a.Category.Name
                    },
                    Category = new Category
                    {
                        Id = a.Category.Id,
                        Name = a.Category.Name
                    },
                }).AsNoTracking();

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

            return await dbContext.Assets
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
                     SchoolId = a.SchoolId,
                     CategoryId = a.CategoryId,
                     RoomLocationId = a.RoomLocationId,
                     RoomLocation = new RoomLocation
                     {
                         Id = a.Category.Id,
                         Name = a.Category.Name,
                         SchoolId = a.SchoolId,
                     },
                     Category = new Category
                     {
                         Id = a.Category.Id,
                         Name = a.Category.Name,
                         SchoolId=a.SchoolId,
                     },
                 })
                 .FirstOrDefaultAsync(asset => asset.Id == assetId);
        }
        public void Update(Asset asset) => dbContext.Assets.Update(asset);
      
        public async Task<bool> Delete(long assetId)
        {
            var asset = await dbContext.Assets.FindAsync(assetId);

            if (asset == null)
                return false;

            asset.Active = false;

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
