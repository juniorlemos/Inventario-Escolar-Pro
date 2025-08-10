using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetAll
{
    public record GetAllCategoriesQuery(int Page, int PageSize) : IRequest<PagedResult<CategoryDto>>;
}
