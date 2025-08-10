using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Register
{
    public record RegisterCategoryCommand(CategoryDto CategoryDto) : IRequest<CategoryDto>;

}
