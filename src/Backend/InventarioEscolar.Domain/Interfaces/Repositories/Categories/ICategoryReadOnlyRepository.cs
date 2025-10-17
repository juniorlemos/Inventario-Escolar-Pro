using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Categories
{
    public interface ICategoryReadOnlyRepository
    {
        Task<bool> ExistCategoryName(string category, long? schoolId);
        Task<Category?> GetById(long categoryId);
        Task<PagedResult<Category>> GetAll(int page, int pageSize, string? searchTerm);
    }
}