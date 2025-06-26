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
            var category = await categoryReadOnlyRepository.GetById(categoryId);

            if (category is null)
                throw new NotFoundException(ResourceMessagesException.CATEGORY_NOT_FOUND);

            await categoryDeleteOnlyRepository.Delete(category.Id);

            await unitOfWork.Commit();
        }
    }
}
