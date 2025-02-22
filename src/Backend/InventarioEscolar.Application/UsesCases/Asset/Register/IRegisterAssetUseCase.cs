using InventarioEscolar.Communication.Request;
using InventarioEscolar.Communication.Response;

namespace InventarioEscolar.Application.UsesCases.Asset.Register
{
    public interface IRegisterAssetUseCase
    {
        public Task<ResponseRegisterAssetJson> Execute(RequestRegisterAssetJson request);
    }
}
