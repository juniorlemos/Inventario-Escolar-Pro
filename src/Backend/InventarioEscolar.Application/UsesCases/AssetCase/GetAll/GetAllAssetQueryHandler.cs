using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Application.UsesCases.AssetCase.GetAll;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Pagination;
using Mapster;
using MediatR;

public class GetAllAssetQueryHandler : IRequestHandler<GetAllAssetQuery, PagedResult<AssetDto>>
{
    private readonly IAssetReadOnlyRepository _assetReadOnlyRepository;

    public GetAllAssetQueryHandler(IAssetReadOnlyRepository assetReadOnlyRepository)
    {
        _assetReadOnlyRepository = assetReadOnlyRepository;
    }

    public async Task<PagedResult<AssetDto>> Handle(GetAllAssetQuery request, CancellationToken cancellationToken)
    {
        var pagedAssets = await _assetReadOnlyRepository.GetAll(request.Page, request.PageSize)
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
