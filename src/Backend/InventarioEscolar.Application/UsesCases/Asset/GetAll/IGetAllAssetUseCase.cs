
using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Response;

namespace InventarioEscolar.Application.UsesCases.Asset.GetAll
{
    public interface IGetAllAssetUseCase
    {
        Task<ResponseAssetJson<IEnumerable<AssetDto>>> Execute();
    }
}
