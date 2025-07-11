using FluentValidation;
using InventarioEscolar.Api.Extension;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.AssetMovements;
using InventarioEscolar.Domain.Repositories.Assets;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Register
{
    public class RegisterAssetMovementUseCase(
        IAssetMovementWriteOnlyRepository assetMovementWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IValidator<AssetMovementDto> validator,
        IAssetReadOnlyRepository assetReadOnlyRepository,
        IAssetUpdateOnlyRepository assetWriteOnlyRepository,
        IRoomLocationUpdateOnlyRepository roomLocationWriteOnlyRepository,
        IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository) : IRegisterAssetMovementUseCase
    {
        public async Task<AssetMovementDto> Execute(AssetMovementDto assetMovementDto)
        {
            await Validate(assetMovementDto);

            if (assetMovementDto.FromRoomId == assetMovementDto.ToRoomId)
                throw new BusinessException(ResourceMessagesException.ASSETMOVEMENT_SAME_ROOM);

            var asset = await assetReadOnlyRepository.GetById(assetMovementDto.AssetId) ?? 
                throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            var roomFrom = await roomLocationReadOnlyRepository.GetById(assetMovementDto.FromRoomId) ?? 
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_ORIGIN);
            
            var roomTo = await roomLocationReadOnlyRepository.GetById(assetMovementDto.ToRoomId) ??
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_DESTINATION);
           
            roomFrom.Assets.Remove(asset);
            roomTo.Assets.Add(asset);
            
            asset.RoomLocation = roomTo;
            asset.RoomLocationId = roomTo.Id;

            assetWriteOnlyRepository.Update(asset);
            roomLocationWriteOnlyRepository.Update(roomFrom);
            roomLocationWriteOnlyRepository.Update(roomTo);

            var assetMovement = assetMovementDto.Adapt<AssetMovement>();

            await assetMovementWriteOnlyRepository.Insert(assetMovement);
            await unitOfWork.Commit();

            return assetMovement.Adapt<AssetMovementDto>();
        }
        private async Task Validate(AssetMovementDto dto)
        {
            var result = await validator.ValidateAsync(dto);

            if (result.IsValid.IsFalse())
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}