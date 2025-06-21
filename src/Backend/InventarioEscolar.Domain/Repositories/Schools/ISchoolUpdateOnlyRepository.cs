using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.Schools
{
    public interface ISchoolUpdateOnlyRepository
    {
      void Update(School school);
    }
}
