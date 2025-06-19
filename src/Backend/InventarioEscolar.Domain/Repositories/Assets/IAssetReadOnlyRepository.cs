using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Repositories.Assets
{
    public interface IAssetReadOnlyRepository
    {
        Task<PagedResult<Asset>> GetAllAssets(int page, int pageSize);
        Task<Entities.Asset?> GetById(long assetId);
        Task<bool> ExistPatrimonyCode(long? patromonyCode);
    }
}
