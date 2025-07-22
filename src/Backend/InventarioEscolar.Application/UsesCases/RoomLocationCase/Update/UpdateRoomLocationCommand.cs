using InventarioEscolar.Communication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.Update
{
    public record UpdateRoomLocationCommand(long Id, UpdateRoomLocationDto RoomLocationDto) : IRequest<Unit>;
}
