using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
