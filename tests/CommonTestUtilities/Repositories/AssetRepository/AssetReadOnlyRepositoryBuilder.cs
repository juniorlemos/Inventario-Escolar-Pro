using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Assets;
using InventarioEscolar.Domain.Pagination;
using NSubstitute;

namespace CommonTestUtilities.Repositories.AssetRepository
{
    public class AssetReadOnlyRepositoryBuilder
    {
        private readonly IAssetReadOnlyRepository _repository;
        public AssetReadOnlyRepositoryBuilder()
        {
            _repository =  Substitute.For<IAssetReadOnlyRepository>();
        }
        public AssetReadOnlyRepositoryBuilder WithAssetExist(long id, Asset asset)
        { 
            _repository.GetById(id).Returns(asset);
            return this;
        }
        public AssetReadOnlyRepositoryBuilder WithAssetNotExist(long id)
        {
            _repository.GetById(id).Returns((Asset?)null);
            return this;
        }
        public AssetReadOnlyRepositoryBuilder WithGetAllReturningNull(int page, int pageSize)
        {
            _repository.GetAll(page, pageSize)!.Returns((PagedResult<Asset>?)null);
            return this;
        }
        public AssetReadOnlyRepositoryBuilder WithAssetExistenceTrue(long? patrimonyCode, long schoolId)
        {
            _repository.ExistPatrimonyCode(patrimonyCode, schoolId).Returns(true);
            return this;
        }
        public AssetReadOnlyRepositoryBuilder WithAssetExistenceFalse(long? patrimonyCode, long schoolId)
        {
            _repository.ExistPatrimonyCode(patrimonyCode, schoolId).Returns(false);
            return this;
        }
        public AssetReadOnlyRepositoryBuilder WithAssetsExist(IEnumerable<Asset> assets, int page = 1, int pageSize = 10)
        {
            var assetList = assets.ToList();
            var skip = (page - 1) * pageSize;
            var pagedItems = assetList.Skip(skip).Take(pageSize).ToList();

            var pagedResult = new PagedResult<Asset>(
                pagedItems,
                assetList.Count,
                page,
                pageSize
            );

            _repository.GetAll(page, pageSize).Returns(pagedResult);

            return this;
        }
        public AssetReadOnlyRepositoryBuilder WithAssetNotFound(long id)
        {
            _repository.GetById(id).Returns((Asset)null!);
            return this;
        }
        public IAssetReadOnlyRepository Build () => _repository;
    }
}