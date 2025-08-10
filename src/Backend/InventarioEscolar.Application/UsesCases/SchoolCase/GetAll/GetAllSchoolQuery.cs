using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetAll
{
    public record GetAllSchoolQuery(int Page, int PageSize) : IRequest<PagedResult<SchoolDto>>;
}
