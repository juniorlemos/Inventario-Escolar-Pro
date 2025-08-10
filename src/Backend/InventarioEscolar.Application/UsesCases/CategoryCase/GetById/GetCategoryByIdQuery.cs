using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetById
{
    public record GetCategoryByIdQuery(long CategoryId) : IRequest<CategoryDto>;
}
