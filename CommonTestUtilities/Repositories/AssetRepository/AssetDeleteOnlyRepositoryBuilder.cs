using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Repositories.AssetRepository
{
    public class AssetDeleteOnlyRepositoryBuilder
    {
        private readonly IAssetDeleteOnlyRepository _repository;
        public AssetDeleteOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IAssetDeleteOnlyRepository>();
        }
        
        public AssetDeleteOnlyRepositoryBuilder WithDeleteReturningTrue(long assetId)
        {
            _repository.Delete(assetId).Returns(true);
            return this;
        }
        public AssetDeleteOnlyRepositoryBuilder WithDeleteReturningFalse(long assetId)
        {
            _repository.Delete(assetId).Returns(false);
            return this;
        }

        public IAssetDeleteOnlyRepository Build ()  => _repository;
    }
}
