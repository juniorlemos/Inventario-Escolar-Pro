using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Register
{
    public record RegisterSchoolCommand(SchoolDto SchoolDto) : IRequest<SchoolDto>;
}
