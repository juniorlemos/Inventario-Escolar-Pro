using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Response;

namespace InventarioEscolar.Application.UsesCases.AssetCase.GetById
{
    public interface IGetByIdAssetUseCase
    {
        Task<AssetDto> Execute(long assetId);
    }
}
