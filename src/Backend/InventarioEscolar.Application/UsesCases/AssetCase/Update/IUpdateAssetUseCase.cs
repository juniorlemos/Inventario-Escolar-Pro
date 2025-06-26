using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public interface IUpdateAssetUseCase
    {
        Task Execute(long id, UpdateAssetDto assetDto);
    }
}
