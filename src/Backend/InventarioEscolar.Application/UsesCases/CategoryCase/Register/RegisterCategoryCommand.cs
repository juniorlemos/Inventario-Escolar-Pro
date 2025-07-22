using InventarioEscolar.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Register
{
    public record RegisterCategoryCommand(CategoryDto CategoryDto) : IRequest<CategoryDto>;

}
