
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.Assets
{
    public interface IAssetUpdateOnlyRepository
    {
     void Update(Asset asset);
    }
}
