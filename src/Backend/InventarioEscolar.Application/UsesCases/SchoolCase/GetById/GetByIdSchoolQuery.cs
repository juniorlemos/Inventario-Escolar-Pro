using InventarioEscolar.Communication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetById
{
    public record GetByIdSchoolQuery(long SchoolId) : IRequest<SchoolDto>;
}
