using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.AssetMovements
{
    public interface IAssetMovementWriteOnlyRepository
    {
        Task Insert(AssetMovement assetMovement);
    }
}
