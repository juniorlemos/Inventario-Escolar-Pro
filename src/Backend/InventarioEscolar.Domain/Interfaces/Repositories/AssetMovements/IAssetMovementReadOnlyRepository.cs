using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements
{
    public interface IAssetMovementReadOnlyRepository
    {
        Task<PagedResult<AssetMovement>> GetAll(int page, int pageSize, bool? isCanceled);
        Task<AssetMovement?> GetById(long assetMovementId);
    }
}
