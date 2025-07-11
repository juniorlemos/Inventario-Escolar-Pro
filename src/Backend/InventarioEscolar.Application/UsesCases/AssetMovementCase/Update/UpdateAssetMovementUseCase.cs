using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.AssetMovements;
using InventarioEscolar.Domain.Repositories.Assets;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Update
{
    public class UpdateAssetMovementUseCase(
         IUnitOfWork unitOfWork,
         IAssetReadOnlyRepository assetReadOnlyRepository,
         IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
         IRoomLocationUpdateOnlyRepository roomLocationUpdateOnlyRepository,
         IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository,
         IAssetUpdateOnlyRepository assetUpdateOnlyRepository,
         IAssetMovementUpdateOnlyRepository assetMovementUpdateOnlyRepository) : IUpdateAssetMovementUseCase
    {
        public async Task Execute(long id, string cancelReason)
        {
            var assetMovement = await assetMovementReadOnlyRepository.GetById(id) ??
                throw new NotFoundException(ResourceMessagesException.ASSETMOVEMENT_NOT_FOUND);
            
            var asset = await assetReadOnlyRepository.GetById(assetMovement.Asset.Id) ??
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);
           
            var roomFrom = await roomLocationReadOnlyRepository.GetById(assetMovement.FromRoom.Id) ??
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_ORIGIN);
            
            var roomTo = await roomLocationReadOnlyRepository.GetById(assetMovement.FromRoom.Id) ??
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_DESTINATION);

            assetMovement.IsCanceled = true;
            assetMovement.CancelReason = cancelReason;
            assetMovement.CanceledAt = DateTime.UtcNow;

            roomTo.Assets.Remove(asset);
            roomFrom.Assets.Add(asset);

            asset.RoomLocationId = roomFrom.Id;
            asset.RoomLocation = roomFrom;

            assetMovementUpdateOnlyRepository.Update(assetMovement);
            assetUpdateOnlyRepository.Update(asset);
            roomLocationUpdateOnlyRepository.Update(roomFrom);
            roomLocationUpdateOnlyRepository.Update(roomTo);

            await unitOfWork.Commit();
        }
    }
}