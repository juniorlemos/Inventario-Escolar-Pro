using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Interfaces.Repositories.Assets
{
    public interface IAssetUpdateOnlyRepository
    {
     void Update(Asset asset);
    }
}
