using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Delete
{
    public record DeleteSchoolCommand(long SchoolId) : IRequest<Unit>;
}
