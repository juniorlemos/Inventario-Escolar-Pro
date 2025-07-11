using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Register
{
    public interface IRegisterAssetMovementUseCase
    {
        Task<AssetMovementDto> Execute(AssetMovementDto assetMovementDto);
    }
}