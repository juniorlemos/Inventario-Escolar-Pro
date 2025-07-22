using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Pagination;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.GetAll
{
    public class GetAllAssetMovementsQueryHandler : IRequestHandler<GetAllAssetMovementsQuery, PagedResult<AssetMovementDto>>
    {
        private readonly IAssetMovementReadOnlyRepository _assetMovementReadOnlyRepository;

        public GetAllAssetMovementsQueryHandler(IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository)
        {
            _assetMovementReadOnlyRepository = assetMovementReadOnlyRepository;
        }

        public async Task<PagedResult<AssetMovementDto>> Handle(GetAllAssetMovementsQuery request, CancellationToken cancellationToken)
        {
            var pagedAssetMovements = await _assetMovementReadOnlyRepository.GetAll(request.Page, request.PageSize, request.IsCanceled)
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
