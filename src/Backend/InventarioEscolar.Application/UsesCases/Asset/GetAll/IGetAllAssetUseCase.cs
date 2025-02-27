using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Application.UsesCases.Asset.GetAll
{
    public interface IGetAllAssetUseCase
    {
        Task <PagedResult<AssetDto>> Execute(int page, int pagesize);
    }
}
