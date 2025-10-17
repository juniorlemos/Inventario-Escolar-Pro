using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Enums;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Assets
{
    public interface IAssetReadOnlyRepository
    {
        Task<PagedResult<Asset>> GetAll(int page,int pageSize,string? searchTerm,ConservationState? conservationState);
        Task<Asset?> GetById(long assetId);
        Task<bool> ExistPatrimonyCode(long? patrimonyCode, long? schoolId);
    }
}