using InventarioEscolar.Communication.Dtos;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetById
{
    public record GetByIdSchoolQuery(long SchoolId) : IRequest<SchoolDto>;
}