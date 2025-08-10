using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Pagination;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll
{
    public class GetAllAssetMovementsQueryHandler(IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository) : IRequestHandler<GetAllAssetMovementsQuery, PagedResult<AssetMovementDto>>
    {
        public async Task<PagedResult<AssetMovementDto>> Handle(GetAllAssetMovementsQuery request, CancellationToken cancellationToken)
        {
            var pagedAssetMovements = await assetMovementReadOnlyRepository.GetAll(request.Page, request.PageSize, request.IsCanceled)
                                      ?? PagedResult<AssetMovement>.Empty(request.Page, request.PageSize);

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