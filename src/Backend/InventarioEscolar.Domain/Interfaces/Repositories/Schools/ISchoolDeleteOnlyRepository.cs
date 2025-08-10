namespace InventarioEscolar.Domain.Interfaces.Repositories.Schools
{
    public interface ISchoolDeleteOnlyRepository
    {
       Task<bool> Delete(long schoolId);
    }
}