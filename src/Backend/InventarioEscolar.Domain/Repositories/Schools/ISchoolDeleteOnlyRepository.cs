namespace InventarioEscolar.Domain.Repositories.Schools
{
    public interface ISchoolDeleteOnlyRepository
    {
       Task<bool> Delete(long schoolId);
    }
}
