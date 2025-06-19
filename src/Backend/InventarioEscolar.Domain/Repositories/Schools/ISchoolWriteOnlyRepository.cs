using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.Schools
{
    public interface ISchoolWriteOnlyRepository
    {
        Task Insert(School school);
    }
}
