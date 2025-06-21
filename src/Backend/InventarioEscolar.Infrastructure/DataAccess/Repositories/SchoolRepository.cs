using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;
using InventarioEscolar.Domain.Repositories.Schools;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class SchoolRepository (InventarioEscolarProDBContext dbContext) : ISchoolWriteOnlyRepository, ISchoolReadOnlyRepository, ISchoolUpdateOnlyRepository, ISchoolDeleteOnlyRepository
    {
        public void Update(School school) => dbContext.Schools.Update(school);

        public async Task<PagedResult<School>> GetAll(int page, int pageSize)
        {
            var query = dbContext.Schools.AsNoTracking();

            var totalCount = await query.CountAsync();

            var items = new List<School>();

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

            return new PagedResult<School>(items, totalCount, page, pageSize);
        }

        public async Task<School?> GetById(long schoolId)
        {
            return await dbContext.Schools.FirstOrDefaultAsync(school => school.Id == schoolId);
        }

        public async Task<School?> GetDuplicateSchool(string name, string? inep, string? address)
        {
            return await dbContext.Schools
            .FirstOrDefaultAsync(s =>
             EF.Functions.Collate(s.Name, "Latin1_General_CI_AS") == name
             || (inep != null && s.Inep == inep)
             || (address != null && s.Address == address));
        }
        public async Task Insert(School school) => await dbContext.Schools.AddAsync(school);

        public async Task<bool> Delete(long schoolId)
        {
            var schools = await dbContext.Schools.FindAsync(schoolId);

            if (schools == null)
                return false;

            dbContext.Schools.Remove(schools);
            return true;
        }
    }
}
