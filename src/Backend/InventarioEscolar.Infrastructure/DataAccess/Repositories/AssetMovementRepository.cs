using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public class AssetMovementRepository(InventarioEscolarProDBContext dbContext) : IAssetMovementReadOnlyRepository, IAssetMovementWriteOnlyRepository, IAssetMovementUpdateOnlyRepository
    {
        public async Task Insert(AssetMovement assetMovement) => await dbContext.AssetMovements.AddAsync(assetMovement);
        public void Update(AssetMovement assetMovement) => dbContext.AssetMovements.Update(assetMovement);
        public async Task<List<AssetMovement>> GetAllWithDetailsAsync()
        {
            return await dbContext.AssetMovements
                .Include(m => m.Asset)
                .Include(m => m.FromRoom)
                .Include(m => m.ToRoom)
                .Where(m => !m.IsCanceled) // se houver soft delete ou cancelamento
                .OrderByDescending(m => m.CanceledAt)
                .ToListAsync();
        }
        public async Task<PagedResult<AssetMovement>> GetAll(int page, int pageSize, bool? isCanceled = null)
        {
            var query = dbContext.AssetMovements.AsQueryable();

            if (isCanceled.HasValue)
            {
                query = query.Where(a => a.IsCanceled == isCanceled.Value);
            }

            var assetMovementsQuery = query.Select(a => new AssetMovement
            {
                    Id = a.Id,
                    MovedAt= a.MovedAt,
                    Responsible = a.Responsible,
                    CanceledAt = a.CanceledAt,
                    CancelReason = a.CancelReason,
                    IsCanceled = a.IsCanceled,
                    Asset = new Asset
                    {
                        Id = a.Asset.Id,
                        Name = a.Asset.Name,
                    },
                    FromRoom  = new RoomLocation
                    {
                        Id = a.FromRoomId,
                        Name = a.FromRoom.Name
                    },
                    ToRoom = new RoomLocation
                    {
                        Id = a.ToRoomId,
                        Name = a.ToRoom.Name
                    },
                }).AsNoTracking();

            var totalCount = await assetMovementsQuery.CountAsync();

            var items = new List<AssetMovement>();

            if (page > 0 && pageSize > 0)
            {
                items = await assetMovementsQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                items = await assetMovementsQuery.ToListAsync();
            }

            return new PagedResult<AssetMovement>(items, totalCount, page, pageSize);
        }

        public async Task<AssetMovement?> GetById(long assetMovementId)
        {
            return await dbContext.AssetMovements
                 .Where(a => a.Id == assetMovementId)
                 .Select(a => new AssetMovement
                 {
                     Id = a.Id,
                     MovedAt = a.MovedAt,
                     Responsible = a.Responsible,
                     Asset = new Asset
                     {
                         Id = a.Asset.Id,
                         Name = a.Asset.Name,
                     },
                     FromRoom = new RoomLocation
                     {
                         Id = a.FromRoomId,
                         Name = a.FromRoom.Name
                     },
                     ToRoom = new RoomLocation
                     {
                         Id = a.ToRoomId,
                         Name = a.ToRoom.Name
                     },
                 })
                 .FirstOrDefaultAsync(assetMovement => assetMovement.Id == assetMovementId);
        }
    }
}
