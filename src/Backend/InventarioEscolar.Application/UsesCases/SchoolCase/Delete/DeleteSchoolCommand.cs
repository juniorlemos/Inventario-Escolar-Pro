using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Delete
{
    public record DeleteSchoolCommand(long SchoolId) : IRequest<Unit>;
}
