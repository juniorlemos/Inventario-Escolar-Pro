namespace InventarioEscolar.Domain.Repositories.Asset
{
    public interface IAssetReadOnlyRepository
    {
        Task<IList<Entities.Asset>> GetAllAssets();
        Task<Entities.Asset?> GetById(Entities.Asset user, long assetId);

    }
}
