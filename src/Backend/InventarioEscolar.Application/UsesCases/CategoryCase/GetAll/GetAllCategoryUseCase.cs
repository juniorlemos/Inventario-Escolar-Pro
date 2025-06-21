using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Repositories.Categories;
using Mapster;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetAll
{
    public class GetAllCategoryUseCase(ICategoryReadOnlyRepository categoryReadOnlyRepository)
        : IGetAllCategoryUseCase
    {
        public async Task<PagedResult<CategoryDto>> Execute(int page, int pageSize)
        {
            var pagedCategories = await categoryReadOnlyRepository.GetAll(page, pageSize)
                         ?? PagedResult<Category>.Empty(page, pageSize);

            var dtoItems = pagedCategories.Items.Adapt<List<CategoryDto>>();

            return new PagedResult<CategoryDto>(
                dtoItems,
                pagedCategories.TotalCount,
                pagedCategories.Page,
                pagedCategories.PageSize
            );
        }
    }
}
