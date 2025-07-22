using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Schools
{
    public interface ISchoolUpdateOnlyRepository
    {
      void Update(School school);
    }
}
