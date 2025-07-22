using InventarioEscolar.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetById
{
    public record GetCategoryByIdQuery(long CategoryId) : IRequest<CategoryDto>;
}
