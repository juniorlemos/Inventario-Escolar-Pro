using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;

namespace CommonTestUtilities.Repositories.ReportsRepository
{
    public class AssetMovementReportReadOnlyRepositoryBuilder
    {
        private readonly IAssetMovementReportReadOnlyRepository _repository;

        public AssetMovementReportReadOnlyRepositoryBuilder()
        {
            _repository = Substitute.For<IAssetMovementReportReadOnlyRepository>();
        }

        public AssetMovementReportReadOnlyRepositoryBuilder WithMovements(IEnumerable<AssetMovement> movements)
        {
            _repository.GetAllAssetMovementsReport()
                       .Returns(Task.FromResult(movements));
            return this;
        }

        public AssetMovementReportReadOnlyRepositoryBuilder WithCanceledMovements(IEnumerable<AssetMovement> movements)
        {
            _repository.GetAllAssetCanceledMovementsReport()
                       .Returns(Task.FromResult(movements));
            return this;
        }
        public IAssetMovementReportReadOnlyRepository Build() => _repository;
    }
}