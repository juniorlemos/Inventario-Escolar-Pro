using InventarioEscolar.Application.Dtos;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public interface IUpdateAssetUseCase
    {
        Task Execute(long id, AssetDto assetDto);
    }
}
