namespace InventarioEscolar.Domain.Repositories.Asset
{
    public interface IAssetReadOnlyRepository
    {
        Task<IEnumerable<Entities.Asset>> GetAllAssets();
        Task<Entities.Asset?> GetById(long assetId);
    }
}
