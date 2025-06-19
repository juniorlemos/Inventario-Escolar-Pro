using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories.Categories;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class CategoryRepository(InventarioEscolarProDBContext dbContext) : ICategoryWriteOnlyRepository, ICategoryReadOnlyRepository
    {
        public async Task<bool> ExistCategoryName(string category)
        {
            return await dbContext.Categories.AnyAsync(s => s.Name.ToUpper() == category.ToUpper());
        }
        public async Task Insert(Category category) => await dbContext.Categories.AddAsync(category);
    }
}
