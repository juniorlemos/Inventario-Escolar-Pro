using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Delete
{
    public record DeleteCategoryCommand(long CategoryId) : IRequest<Unit>;
}