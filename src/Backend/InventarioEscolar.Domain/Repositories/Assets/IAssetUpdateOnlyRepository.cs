
using InventarioEscolar.Domain.Entities;

namespace InventarioEscolar.Domain.Repositories.Assets
{
    public interface IAssetUpdateOnlyRepository
    {
        public void Update(Asset asset);
    }
}
