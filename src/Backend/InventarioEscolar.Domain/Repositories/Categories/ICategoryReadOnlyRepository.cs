namespace InventarioEscolar.Domain.Repositories.Categories
{
    public interface ICategoryReadOnlyRepository
    {
        Task<bool> ExistCategoryName(string category);
    }
}
