using InventarioEscolar.Communication.Request;

namespace InventarioEscolar.Application.UsesCases.Asset.Update
{
    public interface IUpdateAssetUseCase
    {
        public Task Execute(RequestUpdateAssetJson request);
    }
}
