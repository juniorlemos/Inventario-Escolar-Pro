using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.AssetMovements;
using InventarioEscolar.Domain.Pagination;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTestUtilities.Repositories.AssetMovementRepository
{
    public class AssetMovementReadOnlyRepositoryBuilder
    {
        private readonly IAssetMovementReadOnlyRepository _repository;

        public AssetMovementReadOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IAssetMovementReadOnlyRepository>();
        }

        public AssetMovementReadOnlyRepositoryBuilder WithAssetMovementsExist(List<AssetMovement> assetMovements, int page, int pageSize, bool isCanceled)
        {
            var pagedResult = new PagedResult<AssetMovement>(
                assetMovements.GetRange(0, pageSize),
                assetMovements.Count,
                page,
                pageSize
            );

            _repository.GetAll(page, pageSize, isCanceled).Returns(pagedResult);
            return this;
        }

        public AssetMovementReadOnlyRepositoryBuilder WithGetAllReturningNull(int page, int pageSize, bool isCanceled)
        {
            _repository.GetAll(page, pageSize, isCanceled).Returns((PagedResult<AssetMovement>?)null);
            return this;
        }
        public AssetMovementReadOnlyRepositoryBuilder WithAssetMovementExist(long id, AssetMovement assetMovement)
        {
            _repository.GetById(id).Returns(assetMovement);
            return this;
        }

        public AssetMovementReadOnlyRepositoryBuilder WithAssetMovementNotFound(long id)
        {
            _repository.GetById(id).Returns((AssetMovement)null);
            return this;
        }
        public IAssetMovementReadOnlyRepository Build() => _repository;
    }
}
