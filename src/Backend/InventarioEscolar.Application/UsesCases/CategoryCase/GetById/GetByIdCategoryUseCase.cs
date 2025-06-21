using InventarioEscolar.Application.UsesCases.SchoolCase.GetById;
using InventarioEscolar.Communication.Dtos;
using InventarioEscolar.Domain.Repositories.Schools;
using InventarioEscolar.Exceptions.ExceptionsBase;
using InventarioEscolar.Exceptions;
using InventarioEscolar.Domain.Repositories.Categories;
using InventarioEscolar.Application.Dtos;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetById
{
    public class GetByIdCategoryUseCase(
            ICategoryReadOnlyRepository categoryReadOnlyRepository) : IGetByIdCategoryUseCase
    {
        public async Task<CategoryDto> Execute(long categoryId)
        {
            var category = await categoryReadOnlyRepository.GetById(categoryId);

            if (category is null)
                throw new NotFoundException(ResourceMessagesException.SCHOOL_NOT_FOUND);

            return category.Adapt<CategoryDto>();
        }
    }
}
