namespace InventarioEscolar.Application.UsesCases.SchoolCase.Delete
{
    public interface IDeleteSchoolUseCase
    {
        Task Execute(long schoolId);
    }
}
