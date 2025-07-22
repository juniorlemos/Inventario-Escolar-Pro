using InventarioEscolar.Communication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetById
{
    public record GetRoomLocationByIdQuery(long RoomLocationId) : IRequest<RoomLocationDto>;
}
