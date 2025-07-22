using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Update
{
    public class UpdateAssetMovementCommandHandler : IRequestHandler<UpdateAssetMovementCommand,Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssetReadOnlyRepository _assetReadOnlyRepository;
        private readonly IRoomLocationReadOnlyRepository _roomLocationReadOnlyRepository;
        private readonly IRoomLocationUpdateOnlyRepository _roomLocationUpdateOnlyRepository;
        private readonly IAssetMovementReadOnlyRepository _assetMovementReadOnlyRepository;
        private readonly IAssetUpdateOnlyRepository _assetUpdateOnlyRepository;
        private readonly IAssetMovementUpdateOnlyRepository _assetMovementUpdateOnlyRepository;
        private readonly ICurrentUserService _currentUser;

        public UpdateAssetMovementCommandHandler(
            IUnitOfWork unitOfWork,
            IAssetReadOnlyRepository assetReadOnlyRepository,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            IRoomLocationUpdateOnlyRepository roomLocationUpdateOnlyRepository,
            IAssetMovementReadOnlyRepository assetMovementReadOnlyRepository,
            IAssetUpdateOnlyRepository assetUpdateOnlyRepository,
            IAssetMovementUpdateOnlyRepository assetMovementUpdateOnlyRepository,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _assetReadOnlyRepository = assetReadOnlyRepository;
            _roomLocationReadOnlyRepository = roomLocationReadOnlyRepository;
            _roomLocationUpdateOnlyRepository = roomLocationUpdateOnlyRepository;
            _assetMovementReadOnlyRepository = assetMovementReadOnlyRepository;
            _assetUpdateOnlyRepository = assetUpdateOnlyRepository;
            _assetMovementUpdateOnlyRepository = assetMovementUpdateOnlyRepository;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateAssetMovementCommand request, CancellationToken cancellationToken)
        {
            var assetMovement = await _assetMovementReadOnlyRepository.GetById(request.Id)
                ?? throw new NotFoundException(ResourceMessagesException.ASSETMOVEMENT_NOT_FOUND);

            var asset = await _assetReadOnlyRepository.GetById(assetMovement.Asset.Id)
                ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            if (asset.SchoolId != _currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);

            var roomFrom = await _roomLocationReadOnlyRepository.GetById(assetMovement.FromRoom.Id)
                ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_ORIGIN);

            if (roomFrom.SchoolId != _currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);

            var roomTo = await _roomLocationReadOnlyRepository.GetById(assetMovement.ToRoom.Id)  // Corrigido aqui para ToRoom.Id
                ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_DESTINATION);

            if (roomTo.SchoolId != _currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ASSET_NOT_BELONG_TO_SCHOOL);

            assetMovement.IsCanceled = true;
            assetMovement.CancelReason = request.CancelReason;
            assetMovement.CanceledAt = DateTime.UtcNow;

            roomTo.Assets.Remove(asset);
            roomFrom.Assets.Add(asset);

            asset.RoomLocationId = roomFrom.Id;
            asset.RoomLocation = roomFrom;

            _assetMovementUpdateOnlyRepository.Update(assetMovement);
            _assetUpdateOnlyRepository.Update(asset);
            _roomLocationUpdateOnlyRepository.Update(roomFrom);
            _roomLocationUpdateOnlyRepository.Update(roomTo);

            await _unitOfWork.Commit();

            return Unit.Value;
        }
    }
}
