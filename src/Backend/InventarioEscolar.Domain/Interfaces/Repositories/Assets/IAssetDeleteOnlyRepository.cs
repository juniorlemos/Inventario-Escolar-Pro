namespace InventarioEscolar.Domain.Interfaces.Repositories.Assets
{
    public interface IAssetDeleteOnlyRepository
    {
        Task<bool> Delete(long assetId);
    }
}