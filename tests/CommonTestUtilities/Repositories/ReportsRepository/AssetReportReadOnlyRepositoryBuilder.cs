using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;

namespace CommonTestUtilities.Repositories.ReportsRepository
{
    public class AssetReportReadOnlyRepositoryBuilder
    {
        private readonly IAssetReportReadOnlyRepository _repository = Substitute.For<IAssetReportReadOnlyRepository>();
        public AssetReportReadOnlyRepositoryBuilder WithAssets(List<Asset> assets)
        {
            _repository.GetAllAssetReport().Returns(assets);
            return this;
        }
        public IAssetReportReadOnlyRepository Build() => _repository;
    }
}