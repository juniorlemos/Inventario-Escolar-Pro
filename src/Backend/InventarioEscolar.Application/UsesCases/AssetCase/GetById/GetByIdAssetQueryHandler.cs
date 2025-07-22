using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetById
{
    public class GetByIdAssetQueryHandler : IRequestHandler<GetByIdAssetQuery, AssetDto>
    {
        private readonly IAssetReadOnlyRepository _assetReadOnlyRepository;

        public GetByIdAssetQueryHandler(IAssetReadOnlyRepository assetReadOnlyRepository)
        {
            _assetReadOnlyRepository = assetReadOnlyRepository;
        }

        public async Task<AssetDto> Handle(GetByIdAssetQuery request, CancellationToken cancellationToken)
        {
            var asset = await _assetReadOnlyRepository.GetById(request.AssetId);

            if (asset is null)
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            return asset.Adapt<AssetDto>();
        }
    }
}
