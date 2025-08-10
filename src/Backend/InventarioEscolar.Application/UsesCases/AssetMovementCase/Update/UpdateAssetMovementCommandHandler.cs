using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Update
{
    public class UpdateAssetMovementCommandHandler(
        IUnitOfWork unitOfWork,
        IAssetReadOnlyRepository assetReadOnlyRepository,
        IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
        IRoomLocationUpdateOnlyRepository roomLocationUpdateOnlyRepository,
        IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository,
        IAssetUpdateOnlyRepository assetUpdateOnlyRepository,
        IAssetMovementUpdateOnlyRepository assetMovementUpdateOnlyRepository,
        ICurrentUserService currentUser) : IRequestHandler<UpdateAssetMovementCommand,Unit>
    {
        public async Task<Unit> Handle(UpdateAssetMovementCommand request, CancellationToken cancellationToken)
        {
            var assetMovement = await assetMovementReadOnlyRepository.GetById(request.Id)
                ?? throw new NotFoundException(ResourceMessagesException.ASSETMOVEMENT_NOT_FOUND);

            var asset = await assetReadOnlyRepository.GetById(assetMovement.Asset.Id)
                ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            if (asset.SchoolId != currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);

            var roomFrom = await roomLocationReadOnlyRepository.GetById(assetMovement.FromRoom.Id)
                ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_ORIGIN);

            if (roomFrom.SchoolId != currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);

            var roomTo = await roomLocationReadOnlyRepository.GetById(assetMovement.ToRoom.Id)  
                ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_DESTINATION);

            if (roomTo.SchoolId != currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);

            assetMovement.IsCanceled = true;
            assetMovement.CancelReason = request.CancelReason;
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

            return Unit.Value;
        }
    }
}