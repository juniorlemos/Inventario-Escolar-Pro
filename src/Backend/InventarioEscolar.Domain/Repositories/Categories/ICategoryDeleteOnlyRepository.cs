using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.Categories
{
    public interface ICategoryDeleteOnlyRepository
    {
        Task<bool> Delete(long categoryId);
    }
}
