using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.CategoryCase.Update
{
    public interface IUpdateCategoryUseCase
    {
        Task Execute(long id, UploadCategoryDto categoryDto);
    }
}
