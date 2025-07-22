using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Categories
{
    public interface ICategoryWriteOnlyRepository
    {
        Task Insert(Category category);
    }
}
