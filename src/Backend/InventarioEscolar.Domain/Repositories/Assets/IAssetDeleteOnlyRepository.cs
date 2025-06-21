namespace InventarioEscolar.Domain.Repositories.Assets
{
    public interface IAssetDeleteOnlyRepository
    {
        Task<bool> Delete(long assetId);
    }
}
