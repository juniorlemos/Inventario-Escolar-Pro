using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Repositories.Schools;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.Repositories
{
    public sealed class SchoolRepository (InventarioEscolarProDBContext dbContext) : ISchoolWriteOnlyRepository, ISchoolReadOnlyRepository
    {
        public async Task<School?> GetDuplicateSchool(string name, string? inep, string? address)
        {
            return await dbContext.Schools
            .FirstOrDefaultAsync(s =>
             EF.Functions.Collate(s.Name, "Latin1_General_CI_AS") == name
             || (inep != null && s.Inep == inep)
             || (address != null && s.Address == address));
        }
        public async Task Insert(School school) => await dbContext.Schools.AddAsync(school);

    }
}
