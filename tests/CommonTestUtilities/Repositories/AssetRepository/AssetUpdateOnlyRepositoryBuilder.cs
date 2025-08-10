using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using NSubstitute;

namespace CommonTestUtilities.Repositories.AssetRepository
{
    public class AssetUpdateOnlyRepositoryBuilder
    {
        private readonly IAssetUpdateOnlyRepository _repository;
        public AssetUpdateOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IAssetUpdateOnlyRepository>();
        }
        public IAssetUpdateOnlyRepository Build() => _repository;
    }
}