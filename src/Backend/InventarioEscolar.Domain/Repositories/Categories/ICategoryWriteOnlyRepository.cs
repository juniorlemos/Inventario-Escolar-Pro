using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.Categories
{
    public interface ICategoryWriteOnlyRepository
    {
        Task Insert(Category category);
    }
}
