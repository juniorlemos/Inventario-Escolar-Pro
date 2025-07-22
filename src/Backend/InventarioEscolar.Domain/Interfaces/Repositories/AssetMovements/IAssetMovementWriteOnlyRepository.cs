using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements
{
    public interface IAssetMovementWriteOnlyRepository
    {
        Task Insert(AssetMovement assetMovement);
    }
}
