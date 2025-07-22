using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Delete
{
    public record DeleteCategoryCommand(long categoryId) : IRequest<Unit>;
}
