using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using Mapster;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetById
{
    public class GetByIdSchoolQueryHandler(ISchoolReadOnlyRepository schoolReadOnlyRepository) : IRequestHandler<GetByIdSchoolQuery, SchoolDto>
    {
        public async Task<SchoolDto> Handle(GetByIdSchoolQuery request, CancellationToken cancellationToken)
        {
            var school = await schoolReadOnlyRepository.GetById(request.SchoolId);

            if (school is null)
                throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            return school.Adapt<SchoolDto>();
        }
    }
}