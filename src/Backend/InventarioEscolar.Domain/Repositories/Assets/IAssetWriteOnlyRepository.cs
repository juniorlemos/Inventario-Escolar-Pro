using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.Assets
{
    public  interface IAssetWriteOnlyRepository
    {
        Task Insert(Asset asset);
    }
}
