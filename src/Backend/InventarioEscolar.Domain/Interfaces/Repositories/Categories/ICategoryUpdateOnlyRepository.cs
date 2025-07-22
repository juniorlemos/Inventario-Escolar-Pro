using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Categories
{
    public interface ICategoryUpdateOnlyRepository
    {
       void Update(Category category);
    }
}