using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll
{
    public interface IGetAllAssetMovementUseCase
    {
        Task<PagedResult<AssetMovementDto>> Execute(int page, int pagesize, bool? isCanceled);
    }
}