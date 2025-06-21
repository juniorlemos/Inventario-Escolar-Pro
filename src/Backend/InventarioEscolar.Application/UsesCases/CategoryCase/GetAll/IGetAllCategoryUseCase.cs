using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetAll
{
    public interface IGetAllCategoryUseCase
    {
        Task<PagedResult<CategoryDto>> Execute(int page, int pagesize);
    }
}
