using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Response;

namespace InventarioEscolar.Application.UsesCases.Asset.GetById
{
    public interface IGetByIdAssetUseCase
    {
        Task<ResponseAssetJson<AssetDto>> Execute(long id);
    }
}
