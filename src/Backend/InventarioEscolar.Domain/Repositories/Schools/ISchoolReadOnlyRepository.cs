using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.Schools
{
    public interface ISchoolReadOnlyRepository
    {
        Task<School?> GetDuplicateSchool(string name, string? inep , string? address);
    }
}
