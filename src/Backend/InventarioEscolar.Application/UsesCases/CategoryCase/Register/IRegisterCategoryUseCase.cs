using InventarioEscolar.Application.Dtos;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Register
{
    public interface IRegisterCategoryUseCase
    {
        Task<CategoryDto> Execute(CategoryDto categoryDto);
    }
}
