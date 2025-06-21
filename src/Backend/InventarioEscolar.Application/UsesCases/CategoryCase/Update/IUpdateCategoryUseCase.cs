using InventarioEscolar.Application.Dtos;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Update
{
    public interface IUpdateCategoryUseCase
    {
        Task Execute(long id, CategoryDto categoryDto);
    }
}
