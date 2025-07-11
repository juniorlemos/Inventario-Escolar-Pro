using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories;
using InventarioEscolar.Domain.Repositories.Categories;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Exceptions.ExceptionsBase;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Delete
{
    public class DeleteCategoryUseCase(
        ICategoryDeleteOnlyRepository categoryDeleteOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        IUnitOfWork unitOfWork) : IDeleteCategoryUseCase
    {
        public async Task Execute(long categoryId)
        {
            var category = await categoryReadOnlyRepository.GetById(categoryId) ??
                throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);

            if (category.Assets.Any())
                throw new BusinessException(ResourceMessagesException.CATEGORY_HAS_ASSETS);

            await categoryDeleteOnlyRepository.Delete(category.Id);

            await unitOfWork.Commit();
        }
    }
}
