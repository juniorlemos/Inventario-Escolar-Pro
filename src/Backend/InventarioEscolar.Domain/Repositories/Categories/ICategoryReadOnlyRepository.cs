using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Repositories.Categories
{
    public interface ICategoryReadOnlyRepository
    {
        Task<bool> ExistCategoryName(string category);
        Task<Category?> GetById(long categoryId);
        Task<PagedResult<Category>> GetAll(int page, int pageSize);
    }
}
