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
            var school = await schoolReadOnlyRepository.GetById(schoolId) ??
                throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);
            
            if (school.RoomLocations.Any() || school.Assets.Any() || school.Categories.Any() )
                throw new BusinessException(ResourceMessagesException.CATEGORY_HAS_ASSETS);

            await schoolDeleteOnlyRepository.Delete(school.Id);

            await unitOfWork.Commit();
        }
    }
}
