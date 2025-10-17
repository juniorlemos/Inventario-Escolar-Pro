using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class SchoolRepository(InventarioEscolarProDBContext dbContext) : ISchoolWriteOnlyRepository, ISchoolReadOnlyRepository, ISchoolUpdateOnlyRepository, ISchoolDeleteOnlyRepository
    {
        public void Update(School school) => dbContext.Schools.Update(school);
        public async Task<PagedResult<School>> GetAll(int page, int pageSize)
        {
            var query = dbContext.Schools
               .Select(s => new School
               {
                   Id = s.Id,
                   Name = s.Name,
                   Inep = s.Inep,
                   Address = s.Address,
                   City = s.City,
                   RoomLocations = s.RoomLocations
                         .Select(r => new RoomLocation
                         {
                             Id = r.Id,
                             Name = r.Name
                         }).ToList(),
                   Assets = s.Assets
                         .Select(a => new Asset
                         {
                             Id = a.Id,
                             Name = a.Name
                         }).ToList(),
                   Categories = s.Categories
                         .Select(c => new Category
                         {
                             Id = c.Id,
                             Name = c.Name
                         }).ToList()
               })
                .AsNoTracking();

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
            var school = await dbContext.Schools
                .Include(s => s.RoomLocations)
                .Include(s => s.Assets)
                .Include(s => s.Categories)
                .Include(s => s.Users)
                .FirstOrDefaultAsync(s => s.Id == schoolId);

            return school;
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
            var school = await dbContext.Schools.FindAsync(schoolId);

            if (school == null)
                return false;

            school.Active = false;

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}