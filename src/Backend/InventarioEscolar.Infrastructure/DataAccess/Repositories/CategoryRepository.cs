using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Repositories.Categories;
using InventarioEscolar.Domain.Repositories.Schools;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class CategoryRepository(InventarioEscolarProDBContext dbContext) : ICategoryWriteOnlyRepository, ICategoryReadOnlyRepository, ICategoryUpdateOnlyRepository, ICategoryDeleteOnlyRepository
    {
        public void Update(Category category) => dbContext.Categories.Update(category);

        public async Task<PagedResult<Category>> GetAll(int page, int pageSize)
        {
            var query = dbContext.Categories.AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = new List<Category>();

            if (page > 0 && pageSize > 0)
            {
                items = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            else
            {
                items = await query.ToListAsync();
            }

            return new PagedResult<Category>(items, totalCount, page, pageSize);
        }
        public async Task<bool> ExistCategoryName(string category)
        {
            return await dbContext.Categories.AnyAsync(s => s.Name.ToUpper() == category.ToUpper());
        }

        public async Task<Category?> GetById(long categoryId)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(category => category.Id == categoryId);
        }

        public async Task Insert(Category category) => await dbContext.Categories.AddAsync(category);
        public async Task<bool> Delete(long categoryId)
        {
            var categories = await dbContext.Categories.FindAsync(categoryId);

            if (categories == null)
                return false;

            dbContext.Categories.Remove(categories);
            return true;
        }
    }
}
