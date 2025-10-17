using InventarioEscolar.Application.Services.Mappers;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetAll
{
    public class GetAllAssetQueryHandler(IAssetReadOnlyRepository assetReadOnlyRepository)
        : IRequestHandler<GetAllAssetQuery, PagedResult<AssetDto>>
    {
        public async Task<PagedResult<AssetDto>> Handle(GetAllAssetQuery request, CancellationToken cancellationToken)
        {
            var pagedAssets = await assetReadOnlyRepository.GetAll(
                request.Page,
                request.PageSize,
                request.SearchTerm,
                request.ConservationState
            ) ?? PagedResult<Asset>.Empty(request.Page, request.PageSize, request.SearchTerm, request.ConservationState);

            var dtoItems = pagedAssets.Items
                                      .Select(AssetMapper.ToDto)
                                      .ToList();

            return new PagedResult<AssetDto>(
                dtoItems,
                pagedAssets.TotalCount,
                pagedAssets.Page,
                pagedAssets.PageSize,
                pagedAssets.SearchTerm
            );
        }
    }
}
