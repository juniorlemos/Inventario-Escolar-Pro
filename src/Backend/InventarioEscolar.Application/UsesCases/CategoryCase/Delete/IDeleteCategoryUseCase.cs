namespace InventarioEscolar.Application.UsesCases.CategoryCase.Delete
{
    public interface IDeleteCategoryUseCase
    {
        Task Execute(long categoryId);
    }
}
