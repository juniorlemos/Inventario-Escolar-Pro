using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.RoomLocationCase.GetAll
{
    public record GetAllRoomLocationsQuery(int Page, int PageSize, string? SearchTerm) : IRequest<PagedResult<RoomLocationDto>>;
}
