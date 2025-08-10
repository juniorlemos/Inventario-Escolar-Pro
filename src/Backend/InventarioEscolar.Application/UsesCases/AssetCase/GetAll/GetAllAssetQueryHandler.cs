using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Pagination;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetAll
{
    public class GetAllAssetQueryHandler(IAssetReadOnlyRepository assetReadOnlyRepository) : IRequestHandler<GetAllAssetQuery, PagedResult<AssetDto>>
    {
        public async Task<PagedResult<AssetDto>> Handle(GetAllAssetQuery request, CancellationToken cancellationToken)
        {
            var pagedAssets = await assetReadOnlyRepository.GetAll(request.Page, request.PageSize)
                                 ?? PagedResult<Asset>.Empty(request.Page, request.PageSize);

            var dtoItems = pagedAssets.Items.Adapt<List<AssetDto>>();

            return new PagedResult<AssetDto>(
                dtoItems,
                pagedAssets.TotalCount,
                pagedAssets.Page,
                pagedAssets.PageSize
            );
        }
    }
}