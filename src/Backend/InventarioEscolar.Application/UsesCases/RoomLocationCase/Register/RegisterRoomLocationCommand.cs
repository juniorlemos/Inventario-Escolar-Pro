using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Register
{
    public record RegisterRoomLocationCommand(RoomLocationDto RoomLocationDto) : IRequest<RoomLocationDto>;
}
