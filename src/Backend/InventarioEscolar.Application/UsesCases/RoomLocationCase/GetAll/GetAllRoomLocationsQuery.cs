using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll
{
    public record GetAllRoomLocationsQuery(int Page, int PageSize) : IRequest<PagedResult<RoomLocationDto>>;
}
