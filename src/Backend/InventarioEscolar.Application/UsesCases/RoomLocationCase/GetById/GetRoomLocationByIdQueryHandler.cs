using InventarioEscolar.Communication.Dtos;
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

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById
{
    public class GetRoomLocationByIdQueryHandler : IRequestHandler<GetRoomLocationByIdQuery, RoomLocationDto>
    {
        private readonly IRoomLocationReadOnlyRepository _roomLocationReadOnlyRepository;

        public GetRoomLocationByIdQueryHandler(IRoomLocationReadOnlyRepository roomLocationReadOnlyRepository)
        {
            _roomLocationReadOnlyRepository = roomLocationReadOnlyRepository;
        }

        public async Task<RoomLocationDto> Handle(GetRoomLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var roomLocation = await _roomLocationReadOnlyRepository.GetById(request.RoomLocationId);

            if (roomLocation is null)
                throw new NotFoundException(ResourceMessagesException.ROOMLOCATION_NOT_FOUND);

            return roomLocation.Adapt<RoomLocationDto>();
        }
    }
}
