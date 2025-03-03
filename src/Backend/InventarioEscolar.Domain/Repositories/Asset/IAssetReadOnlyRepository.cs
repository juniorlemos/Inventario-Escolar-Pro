using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Repositories.Asset
{
    public interface IAssetReadOnlyRepository
    {
        Task<PagedResult<Entities.Asset>> GetAllAssets(int page, int pageSize);
        Task<Entities.Asset?> GetById(long assetId);
        Task<bool> ExistPatrimonyCode(long? patromonyCode);
    }
}
