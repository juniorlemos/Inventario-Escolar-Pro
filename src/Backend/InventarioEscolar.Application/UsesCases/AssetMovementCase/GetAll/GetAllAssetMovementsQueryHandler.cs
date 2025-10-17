using InventarioEscolar.Application.Services.Mappers;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll
{
    public class GetAllAssetMovementsQueryHandler(IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository)
        : IRequestHandler<GetAllAssetMovementsQuery, PagedResult<AssetMovementDto>>
    {
        public async Task<PagedResult<AssetMovementDto>> Handle(GetAllAssetMovementsQuery request, CancellationToken cancellationToken)
        {
            var pagedAssetMovements = await assetMovementReadOnlyRepository.GetAll(
                request.Page,
                request.PageSize,
                request.SearchTerm,
                request.IsCanceled
            ) ?? PagedResult<AssetMovement>.Empty(request.Page, request.PageSize, request.SearchTerm);

            var dtoItems = pagedAssetMovements.Items
                                              .Select(AssetMovementMapper.ToDto)
                                              .ToList();

            return new PagedResult<AssetMovementDto>(
                dtoItems,
                pagedAssetMovements.TotalCount,
                pagedAssetMovements.Page,
                pagedAssetMovements.PageSize
            );
        }
    }
}