using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Response;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetById
{
    public interface IGetByIdAssetUseCase
    {
        //Task<ResponseAssetJson<AssetDto>> Execute(long id);
        Task Execute(long id);

    }
}
