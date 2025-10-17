using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Mappers;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Register
{
    public class RegisterRoomLocationCommandHandler(
        IRoomLocationWriteOnlyRepository roomLocationWriteOnlyRepository,
        IUnitOfWork unitOfWork,
        IValidator<RoomLocationDto> validator,
        IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
        ICurrentUserService currentUser) : IRequestHandler<RegisterRoomLocationCommand, RoomLocationDto>
    {
        public async Task<RoomLocationDto> Handle(RegisterRoomLocationCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowIfInvalid(request.RoomLocationDto);

            if (!currentUser.IsAuthenticated)
            throw new BusinessException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            var alreadyExists = await roomLocationReadOnlyRepository.ExistRoomLocationName(request.RoomLocationDto.Name, currentUser.SchoolId);

            if (alreadyExists)
                throw new DuplicateEntityException(ResourceMessagesException.ROOMLOCATION_NAME_ALREADY_EXISTS);

            var roomLocation = RoomLocationMapper.ToEntity(request.RoomLocationDto);
            roomLocation.SchoolId = currentUser.SchoolId;

            await roomLocationWriteOnlyRepository.Insert(roomLocation);
            await unitOfWork.Commit();

            return RoomLocationMapper.ToDto(roomLocation);
        }
    }
}