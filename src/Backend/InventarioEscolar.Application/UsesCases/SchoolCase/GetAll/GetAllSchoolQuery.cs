using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetAll
{
    public record GetAllSchoolQuery(int Page, int PageSize) : IRequest<PagedResult<SchoolDto>>;
}
