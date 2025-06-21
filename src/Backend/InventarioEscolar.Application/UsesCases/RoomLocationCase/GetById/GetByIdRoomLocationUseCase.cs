using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById
{
    public class GetByIdRoomLocationUseCase(
            IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository) : IGetByIdRoomLocationUseCase
    {
        public async Task<RoomLocationDto> Execute(long roomLocationId)
        {
            var roomLocation = await roomLocationReadOnlyRepository.GetById(roomLocationId);

            if (roomLocation is null)
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);

            return roomLocation.Adapt<RoomLocationDto>();
        }
    }
}