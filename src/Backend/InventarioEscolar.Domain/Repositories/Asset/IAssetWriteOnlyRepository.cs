namespace InventarioEscolar.Domain.Repositories.Asset
{
    public  interface IAssetWriteOnlyRepository
    {
        Task Add(Entities.Asset asset);
        Task Delete(long assetId);
    }
}
