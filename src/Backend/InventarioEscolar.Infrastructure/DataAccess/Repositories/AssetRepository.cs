using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class AssetRepository(InventarioEscolarProDBContext dbContext) : IAssetReadOnlyRepository, IAssetWriteOnlyRepository, IAssetUpdateOnlyRepository, IAssetDeleteOnlyRepository
    {
        public async Task Insert(Asset asset) => await dbContext.Assets.AddAsync(asset);

        public async Task<bool> ExistPatrimonyCode(long? patrimonyCode, long? schoolId)
        {
            return await dbContext.Assets
                .AnyAsync(p => p.PatrimonyCode == patrimonyCode && p.SchoolId == schoolId);
        }
        public async Task<List<Asset>> GetAllAssetsAsync()
        {
            return await dbContext.Assets
                .Include(a => a.Category)
                .Include(a => a.RoomLocation)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<List<Asset>> GetAllWithConservationStateAsync()
        {
            return await dbContext.Assets
                .Include(a => a.RoomLocation)
                .Include(a => a.Category)
                .Where(a => !a.Active) 
                .ToListAsync();
        }
        public async Task<List<Asset>> GetAllWithCategoryAsync()
        {
            return await dbContext.Assets
                .Include(a => a.Category)
                .Include(a => a.RoomLocation)
                .Where(a => !a.Active)
                .ToListAsync();
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
                        Id = a.RoomLocation.Id,
                        Name = a.RoomLocation.Name
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
                         Id = a.RoomLocation.Id,
                         Name = a.RoomLocation.Name,
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

        public async Task<List<Asset>> GetAllWithConservationStateBySchoolAsync()
        {
            return await dbContext.Assets
        .Include(a => a.School) // inclui dados da escola (opcional)
        .Where(a => a.ConservationState != null) // garante que o estado está definido
        .OrderBy(a => a.School.Name)             // opcional: organiza por escola
        .ThenBy(a => a.ConservationState)        // opcional: organiza por estado
        .ToListAsync();
        }

        public async Task<List<Asset>> GetAllAssetsWithLocationAsync()
        {
            return await dbContext.Assets
                .Include(a => a.RoomLocation)  // inclui a localização do bem
                .Include(a => a.School)        // opcional: útil se quiser saber de qual escola é o bem
                .Where(a => a.RoomLocationId != null) // somente os que têm localização definida
                .OrderBy(a => a.School.Name)
                .ThenBy(a => a.RoomLocation.Name)
                .ThenBy(a => a.Name)
                .ToListAsync();
        }

    }
}
