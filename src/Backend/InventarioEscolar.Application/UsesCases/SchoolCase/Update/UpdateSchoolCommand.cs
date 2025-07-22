using InventarioEscolar.Communication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Update
{
    public record UpdateSchoolCommand(long Id, UpdateSchoolDto SchoolDto) : IRequest<Unit>;
}
