using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete
{
    public class DeleteRoomLocationUseCase(
        IRoomLocationDeleteOnlyRepository roomLocationDeleteOnlyRepository,
        IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository,
        IUnitOfWork unitOfWork) : IDeleteRoomLocationUseCase
    {
        public async Task Execute(long roomLocationId)
        {
            var roomLocation = await roomLocationReadOnlyRepository.GetById(roomLocationId) ??
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);
           
            if (roomLocation.Assets.Any())
                throw new BusinessException(ResourceMessagesException.ROOMLOCATION_HAS_ASSETS);

            await roomLocationDeleteOnlyRepository.Delete(roomLocation.Id);

            await unitOfWork.Commit();
        }
    }
}
