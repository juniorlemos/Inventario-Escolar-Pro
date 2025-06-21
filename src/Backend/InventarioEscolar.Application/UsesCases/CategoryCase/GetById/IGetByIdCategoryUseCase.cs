using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.GetById
{
    public interface IGetByIdCategoryUseCase
    {
        Task<CategoryDto> Execute(long categoryId);
    }
}
