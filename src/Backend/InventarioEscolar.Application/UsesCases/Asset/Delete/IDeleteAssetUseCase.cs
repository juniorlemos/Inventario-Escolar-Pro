namespace InventarioEscolar.Application.UsesCases.Asset.Delete
{
    public interface IDeleteAssetUseCase
    {
        Task Execute(long assetId);
    }
}
