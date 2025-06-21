using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetById
{
    public class GetByIdAssetUseCase(
            IAssetReadOnlyRepository assetReadOnlyRepository) : IGetByIdAssetUseCase
    {
        public async Task<AssetDto> Execute(long assetId)
        {
            var asset = await assetReadOnlyRepository.GetById(assetId);

            if (asset is null)
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            return asset.Adapt<AssetDto>();
        }
    }
}
