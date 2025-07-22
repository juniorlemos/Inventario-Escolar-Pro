using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Delete
{
    public record DeleteRoomLocationCommand(long RoomLocationId) : IRequest<Unit>;
}
