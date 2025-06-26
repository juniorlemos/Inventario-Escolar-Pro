
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Assets;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Delete
{
    public class DeleteAssetUseCase(
        IAssetDeleteOnlyRepository assetDeleteOnlyRepository,
        IAssetReadOnlyRepository assetReadOnlyRepository,
        IUnitOfWork unitOfWork) : IDeleteAssetUseCase
    {
        public async Task Execute(long assetId)
        {
            var asset = await assetReadOnlyRepository.GetById(assetId);

            if (asset is null)
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            await assetDeleteOnlyRepository.Delete(asset.Id);

            await unitOfWork.Commit();
        }
    }
}