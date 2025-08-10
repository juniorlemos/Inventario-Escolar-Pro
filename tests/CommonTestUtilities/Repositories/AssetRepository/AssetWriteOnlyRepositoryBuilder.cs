using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using NSubstitute;

namespace CommonTestUtilities.Repositories.AssetRepository
{
    public class AssetWriteOnlyRepositoryBuilder
    {
        private readonly IAssetWriteOnlyRepository _repository;
        public AssetWriteOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IAssetWriteOnlyRepository>();
        }
        public IAssetWriteOnlyRepository Build() => _repository;
    }
}
