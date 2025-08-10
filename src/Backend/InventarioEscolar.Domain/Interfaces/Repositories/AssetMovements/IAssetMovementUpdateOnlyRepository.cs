using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements
{
    public interface IAssetMovementUpdateOnlyRepository
    {
        void Update(AssetMovement assetMovement);
    }
}