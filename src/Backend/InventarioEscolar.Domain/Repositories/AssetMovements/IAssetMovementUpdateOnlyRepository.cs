using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.AssetMovements
{
    public interface IAssetMovementUpdateOnlyRepository
    {
        void Update(AssetMovement assetMovement);
    }
}
