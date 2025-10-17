using FluentValidation;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Validators;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Update
{
    public class UpdateRoomLocationCommandHandler(
        IUnitOfWork unitOfWork,
        IValidator<UpdateRoomLocationDto> validator,
        IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
        IRoomLocationUpdateOnlyRepository roomLocationUpdateOnlyRepository,
        ICurrentUserService currentUser) : IRequestHandler<UpdateRoomLocationCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateRoomLocationCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowIfInvalid(request.RoomLocationDto);

            var roomLocation = await roomLocationReadOnlyRepository.GetById(request.Id);

            if (roomLocation is null)
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);

            if (roomLocation.SchoolId != currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);

            request.RoomLocationDto.Adapt(roomLocation);
            roomLocation.SchoolId = currentUser.SchoolId;

            roomLocationUpdateOnlyRepository.Update(roomLocation);
            await unitOfWork.Commit();

            return Unit.Value;
        }
    }
}