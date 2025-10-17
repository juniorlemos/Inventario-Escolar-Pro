using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Enums;
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
        public async Task<PagedResult<Asset>> GetAll(
                  int page,
                  int pageSize,
                  string? searchTerm = null,
                  ConservationState? conservationState = null)
        {
            page = Math.Max(page, 1);

            var query = dbContext.Assets
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
                    RoomLocation = a.RoomLocation != null
                        ? new RoomLocation { Id = a.RoomLocation.Id, Name = a.RoomLocation.Name }
                        : null,
                    Category = a.Category != null
                        ? new Category { Id = a.Category.Id, Name = a.Category.Name }
                        : null
                });

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = $"%{searchTerm.ToLower()}%";
                query = query.Where(a =>
                    EF.Functions.Like(a.Name.ToLower(), normalizedSearch) ||
                    EF.Functions.Like(a.PatrimonyCode.ToString(), normalizedSearch));
            }

            if (conservationState.HasValue)
            {
                query = query.Where(a => a.ConservationState == conservationState.Value);
            }

            var totalCount = await query.CountAsync();

            List<Asset> items;

            if (pageSize > 0)
            {
                items = await query
                    .OrderBy(a => a.Name)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                items = await query
                    .OrderBy(a => a.Name)
                    .ToListAsync();
            }

            return new PagedResult<Asset>(items, totalCount, page, pageSize, searchTerm);
        }

        public async Task<Asset?> GetById(long assetId)
        {
            return await dbContext.Assets
                .Include(a => a.RoomLocation)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == assetId);
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
        .Include(a => a.School) 
        .Where(a => a.ConservationState != null) 
        .OrderBy(a => a.School.Name)             
        .ThenBy(a => a.ConservationState)       
        .ToListAsync();
        }
        public async Task<List<Asset>> GetAllAssetsWithLocationAsync()
        {
            return await dbContext.Assets
                .Include(a => a.RoomLocation)  
                .Include(a => a.School)        
                .Where(a => a.RoomLocationId != null) 
                .OrderBy(a => a.School.Name)
                .ThenBy(a => a.RoomLocation.Name)
                .ThenBy(a => a.Name)
                .ToListAsync();
        }
    }
}