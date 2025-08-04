using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
