using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Domain.Interfaces;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;
using MediatR;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Delete
{
    public class DeleteCategoryCommandHandler(
        ICategoryDeleteOnlyRepository categoryDeleteOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser) : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await categoryReadOnlyRepository.GetById(request.CategoryId)
                ?? throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);

            if (category.Assets.Any())
                throw new BusinessException(ResourceMessagesException.CATEGORY_HAS_ASSETS);

            if (category.SchoolId != currentUser.SchoolId)
                throw new BusinessException(ResourceMessagesException.CATEGORY_NOT_BELONG_TO_SCHOOL);

            await categoryDeleteOnlyRepository.Delete(category.Id);
            await unitOfWork.Commit();

            return Unit.Value;
        }
    }
}