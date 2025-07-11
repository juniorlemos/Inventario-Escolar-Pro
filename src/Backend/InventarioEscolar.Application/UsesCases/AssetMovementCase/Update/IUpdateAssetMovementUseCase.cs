using InventarioEscolar.Communication.Dtos;

namespace InventarioEscolar.Application.UsesCases.AssetMovementCase.Update
{
    public interface IUpdateAssetMovementUseCase
    {
        Task Execute(long id, string cancelReason);
    }
}
