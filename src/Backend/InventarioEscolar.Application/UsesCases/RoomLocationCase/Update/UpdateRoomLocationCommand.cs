using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Update
{
    public record UpdateRoomLocationCommand(long Id, UpdateRoomLocationDto RoomLocationDto) : IRequest<Unit>;
}
