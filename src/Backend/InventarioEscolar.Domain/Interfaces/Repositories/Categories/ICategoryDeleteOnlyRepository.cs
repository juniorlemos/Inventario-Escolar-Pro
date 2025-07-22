using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Categories
{
    public interface ICategoryDeleteOnlyRepository
    {
        Task<bool> Delete(long categoryId);
    }
}
