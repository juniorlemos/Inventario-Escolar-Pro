using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Assets
{
    public  interface IAssetWriteOnlyRepository
    {
        Task Insert(Asset asset);
    }
}
