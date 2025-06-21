using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.Categories
{
    public interface ICategoryUpdateOnlyRepository
    {
       void Update(Category category);
    }
}