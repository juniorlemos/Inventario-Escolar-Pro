using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.RoomLocations;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById
{
    public class GetRoomLocationByIdQueryHandler(IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository) : IRequestHandler<GetRoomLocationByIdQuery, RoomLocationDto>
    {
        public async Task<RoomLocationDto> Handle(GetRoomLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var roomLocation = await roomLocationReadOnlyRepository.GetById(request.RoomLocationId);

            if (roomLocation is null)
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);

            return roomLocation.Adapt<RoomLocationDto>();
        }
    }
}
