using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete
{
    public class DeleteRoomLocationCommandHandler(
        IRoomLocationDeleteOnlyRepository roomLocationDeleteOnlyRepository,
        IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser) : IRequestHandler<DeleteRoomLocationCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteRoomLocationCommand request, CancellationToken cancellationToken)
        {
            var roomLocation = await roomLocationReadOnlyRepository.GetById(request.RoomLocationId)
                ?? throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);

            if (roomLocation.Assets.Any())
                throw new BusinessException(ResourceMessagesException.ROOMLOCATION_HAS_ASSETS);

            if (roomLocation.SchoolId != currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.ROOMLOCATION_NOT_BELONG_TO_SCHOOL);

            await roomLocationDeleteOnlyRepository.Delete(roomLocation.Id);
            await unitOfWork.Commit();

            return Unit.Value;
        }
    }
}