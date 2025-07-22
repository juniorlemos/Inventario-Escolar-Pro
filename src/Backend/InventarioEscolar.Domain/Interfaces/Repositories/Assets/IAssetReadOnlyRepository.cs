using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Assets
{
    public interface IAssetReadOnlyRepository
    {
        Task<PagedResult<Asset>> GetAll(int page, int pageSize);
        Task<Asset?> GetById(long assetId);
        Task<bool> ExistPatrimonyCode(long? patrimonyCode, long? schoolId);
        Task<List<Asset>> GetAllAssetsAsync();
        Task<List<Asset>> GetAllWithConservationStateBySchoolAsync();
        Task<List<Asset>> GetAllWithCategoryAsync();
        Task<List<Asset>> GetAllAssetsWithLocationAsync();
    }
}
