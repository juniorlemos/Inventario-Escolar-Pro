using InventarioEscolar.Exceptions.ExceptionsBase;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Domain.Repositories.Schools;
using InventarioEscolar.Communication.Dtos;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.GetById
{
    public class GetByIdSchoolUseCase(
            ISchoolReadOnlyRepository schoolReadOnlyRepository) : IGetByIdSchoolUseCase
    {
        public async Task<SchoolDto> Execute(long schoolId)
        {
            var school = await schoolReadOnlyRepository.GetById(schoolId);

            if (school is null)
                throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            return school.Adapt<SchoolDto>();
        }
    }
}
