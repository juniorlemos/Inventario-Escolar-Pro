using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Pagination;

namespace InventarioEscolar.Domain.Repositories.Schools
{
    public interface ISchoolReadOnlyRepository
    {
        Task<School?> GetDuplicateSchool(string name, string? inep , string? address);
        Task<School?> GetById(long schoolId);
        Task<PagedResult<School>> GetAll(int page, int pageSize);
    }
}
