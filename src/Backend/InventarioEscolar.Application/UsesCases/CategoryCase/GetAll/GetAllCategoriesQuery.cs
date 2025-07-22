using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetAll
{
    public record GetAllCategoriesQuery(int Page, int PageSize) : IRequest<PagedResult<CategoryDto>>;
}
