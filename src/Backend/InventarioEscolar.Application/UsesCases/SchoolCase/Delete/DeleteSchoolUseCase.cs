using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Exceptions.ExceptionsBase;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Domain.Repositories.Schools;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Delete
{
    public class DeleteSchoolUseCase(
        ISchoolDeleteOnlyRepository schoolDeleteOnlyRepository,
        ISchoolReadOnlyRepository schoolReadOnlyRepository,
        IUnitOfWork unitOfWork) : IDeleteSchoolUseCase
    {
        public async Task Execute(long schoolId)
        {
            var school = await schoolReadOnlyRepository.GetById(schoolId);

            if (school is null)
                throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            await schoolDeleteOnlyRepository.Delete(school.Id);

            await unitOfWork.Commit();
        }
    }
}
