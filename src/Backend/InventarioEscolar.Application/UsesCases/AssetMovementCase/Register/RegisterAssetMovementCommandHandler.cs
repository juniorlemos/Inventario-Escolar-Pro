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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Register
{
    public class RegisterAssetMovementCommandHandler : IRequestHandler<RegisterAssetMovementCommand, AssetMovementDto>
    {
        private readonly IAssetMovementWriteOnlyRepository _assetMovementWriteOnlyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AssetMovementDto> _validator;
        private readonly IAssetReadOnlyRepository _assetReadOnlyRepository;
        private readonly IAssetUpdateOnlyRepository _assetWriteOnlyRepository;
        private readonly IRoomLocationUpdateOnlyRepository _roomLocationWriteOnlyRepository;
        private readonly IRoomLocationReadOnlyRepository _roomLocationReadOnlyRepository;
        private readonly ICurrentUserService _currentUser;

        public RegisterAssetMovementCommandHandler(
            IAssetMovementWriteOnlyRepository assetMovementWriteOnlyRepository,
            IUnitOfWork unitOfWork,
            IValidator<AssetMovementDto> validator,
            IAssetReadOnlyRepository assetReadOnlyRepository,
            IAssetUpdateOnlyRepository assetWriteOnlyRepository,
            IRoomLocationUpdateOnlyRepository roomLocationWriteOnlyRepository,
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
            ICurrentUserService currentUser)
        {
            _assetMovementWriteOnlyRepository = assetMovementWriteOnlyRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _assetReadOnlyRepository = assetReadOnlyRepository;
            _assetWriteOnlyRepository = assetWriteOnlyRepository;
            _roomLocationWriteOnlyRepository = roomLocationWriteOnlyRepository;
            _roomLocationReadOnlyRepository = roomLocationReadOnlyRepository;
            _currentUser = currentUser;
        }

        public async Task<AssetMovementDto> Handle(RegisterAssetMovementCommand request, CancellationToken cancellationToken)
        {
            await Validate(request.AssetMovementDto);

            var schoolId = _currentUser.SchoolId
                           ?? throw new BusinessException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            if (request.AssetMovementDto.FromRoomId == request.AssetMovementDto.ToRoomId)
                throw new BusinessException(ResourceMessagesException.ASSETMOVEMENT_SAME_ROOM);

            var asset = await _assetReadOnlyRepository.GetById(request.AssetMovementDto.AssetId)
                        ?? throw new NotFoundException(ResourceMessagesException.ASSET_NOT_FOUND);

            var roomFrom = await _roomLocationReadOnlyRepository.GetById(request.AssetMovementDto.FromRoomId)
                           ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_ORIGIN);

            var roomTo = await _roomLocationReadOnlyRepository.GetById(request.AssetMovementDto.ToRoomId)
                         ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND_DESTINATION);

            roomFrom.Assets.Remove(asset);
            roomTo.Assets.Add(asset);

            asset.RoomLocation = roomTo;
            asset.RoomLocationId = roomTo.Id;

            _assetWriteOnlyRepository.Update(asset);
            _roomLocationWriteOnlyRepository.Update(roomFrom);
            _roomLocationWriteOnlyRepository.Update(roomTo);

            var assetMovement = request.AssetMovementDto.Adapt<AssetMovement>();

            await _assetMovementWriteOnlyRepository.Insert(assetMovement);
            await _unitOfWork.Commit();

            return assetMovement.Adapt<AssetMovementDto>();
        }

        private async Task Validate(AssetMovementDto dto)
        {
            var result = await _validator.ValidateAsync(dto);

            if (!result.IsValid)
            {
                throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
            }
        }
    }
}
