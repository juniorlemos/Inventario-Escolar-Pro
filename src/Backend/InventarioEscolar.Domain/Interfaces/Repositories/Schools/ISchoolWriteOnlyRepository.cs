using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Schools
{
    public interface ISchoolWriteOnlyRepository
    {
        Task Insert(School school);
    }
}
