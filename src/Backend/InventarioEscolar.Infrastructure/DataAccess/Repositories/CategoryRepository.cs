using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Categories;
using InventarioEscolar.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class CategoryRepository(InventarioEscolarProDBContext dbContext) : ICategoryWriteOnlyRepository, ICategoryReadOnlyRepository, ICategoryUpdateOnlyRepository, ICategoryDeleteOnlyRepository
    {
        public void Update(Category category) => dbContext.Categories.Update(category);

        public async Task<PagedResult<Category>> GetAll(int page, int pageSize, string? searchTerm)
        {
            var query = dbContext.Categories
                 .Select(c => new Category
                 {
                     Id = c.Id,
                     Name = c.Name,
                     Description= c.Description,
                     Assets = c.Assets
                         .Select(a => new Asset
                         {
                             Id = a.Id,
                             Name = a.Name
                         }).ToList()
                 })
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalizedSearch = searchTerm.ToLower();

                query = query.Where(a =>
                    a.Name.ToLower().Contains(normalizedSearch));
            }

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
        public async Task<bool> ExistCategoryName(string category, long? schoolId)
        {
            return await dbContext.Categories.AnyAsync(s => s.Name.ToUpper() == category.ToUpper() && s.SchoolId == schoolId);
        }
        public async Task<Category?> GetById(long categoryId)
        {
            return await dbContext.Categories
                .Include(c => c.Assets)
                .Include(c => c.School) 
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task Insert(Category category) => await dbContext.Categories.AddAsync(category);
        public async Task<bool> Delete(long categoryId)
        {
            var category = await dbContext.Categories.FindAsync(categoryId);

            if (category == null)
                return false;

            category.Active = false;

            await dbContext.SaveChangesAsync(); 
            return true;
        }
    }
}