using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Mappers;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Register
{
    public class RegisterAssetMovementCommandHandler(
        IAssetMovementWriteOnlyRepository assetMovementWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IValidator<AssetMovementDto> validator,
        IAssetReadOnlyRepository assetReadOnlyRepository,
        IAssetUpdateOnlyRepository assetWriteOnlyRepository,
        IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
        ICurrentUserService currentUser) : IRequestHandler<RegisterAssetMovementCommand, Unit>
    {
        public async Task<Unit> Handle(RegisterAssetMovementCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request.AssetMovementDto, cancellationToken);

            if (!currentUser.IsAuthenticated)
                throw new BusinessException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            var asset = await assetReadOnlyRepository.GetById(request.AssetMovementDto.AssetId)
                        ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            var roomFrom = await roomLocationReadOnlyRepository.GetById(asset.RoomLocationId!.Value)
                           ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_ORIGIN);

            var roomTo = await roomLocationReadOnlyRepository.GetById(request.AssetMovementDto.ToRoomId)
                         ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_DESTINATION);

            if (roomFrom.Id == roomTo.Id)
                throw new BusinessException(ResourceMessagesException.ASSETMOVEMENT_SAME_ROOM);

            asset.RoomLocationId = roomTo.Id;
            asset.RoomLocation = null; 

            assetWriteOnlyRepository.Update(asset);

            var assetMovement = AssetMovementMapper.ToEntity(request.AssetMovementDto);
            assetMovement.AssetId = asset.Id;
            assetMovement.Asset = asset; 

            assetMovement.FromRoomId = roomFrom.Id;
            assetMovement.FromRoom = null!; 

            assetMovement.ToRoomId = roomTo.Id;
            assetMovement.ToRoom = null!; 

            assetMovement.SchoolId = currentUser.SchoolId;

            await assetMovementWriteOnlyRepository.Insert(assetMovement);

            await unitOfWork.Commit();

            return Unit.Value;
        }

    }
}
