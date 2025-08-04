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
