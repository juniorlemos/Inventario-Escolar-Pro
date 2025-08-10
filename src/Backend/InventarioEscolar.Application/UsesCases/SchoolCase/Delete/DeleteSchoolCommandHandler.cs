using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.SchoolCase.Delete
{
    public class DeleteSchoolCommandHandler(
        ISchoolDeleteOnlyRepository schoolDeleteOnlyRepository,
        ISchoolReadOnlyRepository schoolReadOnlyRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteSchoolCommand,Unit>
    {
        public async Task<Unit> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = await schoolReadOnlyRepository.GetById(request.SchoolId)
                ?? throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            if (school.RoomLocations.Any() || school.Assets.Any() || school.Categories.Any())
                throw new BusinessException(ResourceMessagesException.CATEGORY_HAS_ASSETS);

            await schoolDeleteOnlyRepository.Delete(school.Id);

            await unitOfWork.Commit();

            return Unit.Value;
        }
    }
}
