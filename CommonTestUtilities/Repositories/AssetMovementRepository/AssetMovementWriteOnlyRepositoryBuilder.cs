using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
