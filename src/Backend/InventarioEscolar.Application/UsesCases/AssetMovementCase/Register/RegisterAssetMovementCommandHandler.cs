using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Register
{
    public class RegisterAssetMovementCommandHandler(
        IAssetMovementWriteOnlyRepository assetMovementWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IValidator<AssetMovementDto> validator,
        IAssetReadOnlyRepository assetReadOnlyRepository,
        IAssetUpdateOnlyRepository assetWriteOnlyRepository,
        IRoomLocationUpdateOnlyRepository roomLocationWriteOnlyRepository,
        IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
        ICurrentUserService currentUser) : IRequestHandler<RegisterAssetMovementCommand, AssetMovementDto>
    {
        public async Task<AssetMovementDto> Handle(RegisterAssetMovementCommand request, CancellationToken cancellationToken)
        {
            await Validate(request.AssetMovementDto);

            if (!currentUser.IsAuthenticated)
                   throw new BusinessException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            if (request.AssetMovementDto.FromRoomId == request.AssetMovementDto.ToRoomId)
                throw new BusinessException(ResourceMessagesException.ASSETMOVEMENT_SAME_ROOM);

            var asset = await assetReadOnlyRepository.GetById(request.AssetMovementDto.AssetId)
                        ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            var roomFrom = await roomLocationReadOnlyRepository.GetById(request.AssetMovementDto.FromRoomId)
                           ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_ORIGIN);

            var roomTo = await roomLocationReadOnlyRepository.GetById(request.AssetMovementDto.ToRoomId)
                         ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_DESTINATION);

            roomFrom.Assets.Remove(asset);
            roomTo.Assets.Add(asset);

            asset.RoomLocation = roomTo;
            asset.RoomLocationId = roomTo.Id;

            assetWriteOnlyRepository.Update(asset);
            roomLocationWriteOnlyRepository.Update(roomFrom);
            roomLocationWriteOnlyRepository.Update(roomTo);

            var assetMovement = request.AssetMovementDto.Adapt<AssetMovement>();

            await assetMovementWriteOnlyRepository.Insert(assetMovement);
            await unitOfWork.Commit();

            return assetMovement.Adapt<AssetMovementDto>();
        }

        private async Task Validate(AssetMovementDto dto)
        {
            var result = await validator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}