
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Repositories.Assets;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetAll
{
    public class GetAllAssetUseCase(IAssetReadOnlyRepository assetReadOnlyRepository)
        : IGetAllAssetUseCase
    {
        public async Task<PagedResult<AssetDto>> Execute(int page, int pageSize)
        {
            var pagedAssets = await assetReadOnlyRepository.GetAll(page, pageSize)
                         ?? PagedResult<Asset>.Empty(page, pageSize);

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
