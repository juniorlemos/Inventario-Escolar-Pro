using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Repositories.AssetMovements;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll
{
    public class GetAllAssetMovementUseCase(
        IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository) : IGetAllAssetMovementUseCase
    {
        public async Task<PagedResult<AssetMovementDto>> Execute(int page, int pageSize, bool? isCanceled = null)
        {
            var pagedAssetMovements = await assetMovementReadOnlyRepository.GetAll(page, pageSize, isCanceled) ??
                PagedResult<AssetMovement>.Empty(page, pageSize);

            var dtoItems = pagedAssetMovements.Items.Adapt<List<AssetMovementDto>>();

            return new PagedResult<AssetMovementDto>(
                dtoItems,
                pagedAssetMovements.TotalCount,
                pagedAssetMovements.Page,
                pagedAssetMovements.PageSize
            );
        }
    }
}