namespace InventarioEscolar.Application.UsesCases.AssetCase.Delete
{
    public interface IDeleteAssetUseCase
    {
        Task Execute(long assetId);
    }
}
