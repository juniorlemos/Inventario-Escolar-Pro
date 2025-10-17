using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetSchollData
{
    public record GetSchoolDataQuery() : IRequest<SchoolDto>;
}
