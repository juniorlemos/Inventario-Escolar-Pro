using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Update
{
    public record UpdateCategoryCommand(long Id, UploadCategoryDto CategoryDto) : IRequest<Unit>;
}
