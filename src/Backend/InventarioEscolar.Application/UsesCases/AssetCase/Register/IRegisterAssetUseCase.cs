using InventarioEscolar.Application.Dtos;
using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Register
{
    public interface IRegisterAssetUseCase
    {
       Task<AssetDto> Execute(AssetDto assetDto);
    }
}
