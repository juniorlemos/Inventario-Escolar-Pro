using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Update
{
    public record UpdateSchoolCommand(long Id, UpdateSchoolDto SchoolDto) : IRequest<Unit>;
}
