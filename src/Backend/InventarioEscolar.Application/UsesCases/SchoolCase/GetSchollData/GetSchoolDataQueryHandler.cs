using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.Services.Mappers;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetSchollData
{
    public class GetSchoolDataQueryHandler(
        ISchoolReadOnlyRepository schoolReadOnlyRepository,
                                  ICurrentUserService currentUser
        ) : IRequestHandler<GetSchoolDataQuery, SchoolDto>
    {

        public async Task<SchoolDto> Handle(GetSchoolDataQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated)
                throw new BusinessException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            var schoolId = currentUser.SchoolId;

            var school = await schoolReadOnlyRepository.GetById(schoolId);

            if (school is null)
                throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            return SchoolMapper.ToDto(school);
        }
    }
}