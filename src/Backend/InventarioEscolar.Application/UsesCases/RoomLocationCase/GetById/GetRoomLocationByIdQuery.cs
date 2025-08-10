using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById
{
    public record GetRoomLocationByIdQuery(long RoomLocationId) : IRequest<RoomLocationDto>;
}
