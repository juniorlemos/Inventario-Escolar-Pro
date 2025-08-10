using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using NSubstitute;

namespace CommonTestUtilities.Repositories.AssetMovementRepository
{
    public class AssetMovementUpdateOnlyRepositoryBuilder
    {

        private readonly IAssetMovementUpdateOnlyRepository _repository;
        public AssetMovementUpdateOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IAssetMovementUpdateOnlyRepository>();
        }
        public IAssetMovementUpdateOnlyRepository Build() => _repository;
    }
}