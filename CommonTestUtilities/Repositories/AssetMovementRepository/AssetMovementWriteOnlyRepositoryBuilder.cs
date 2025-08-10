using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using NSubstitute;

namespace CommonTestUtilities.Repositories.AssetMovementRepository
{
    public class AssetMovementWriteOnlyRepositoryBuilder
    {
        private readonly IAssetMovementWriteOnlyRepository _repository;
        public AssetMovementWriteOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IAssetMovementWriteOnlyRepository>();
        }
        public IAssetMovementWriteOnlyRepository Build() => _repository;
    }
}