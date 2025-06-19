using InventarioEscolar.Communication.Request;

namespace InventarioEscolar.Application.UsesCases.AssetCase.Update
{
    public interface IUpdateAssetUseCase
    {
        public Task Execute(RequestUpdateAssetJson request);
    }
}
