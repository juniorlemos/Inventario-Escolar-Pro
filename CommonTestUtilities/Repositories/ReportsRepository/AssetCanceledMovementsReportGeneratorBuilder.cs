using InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase;
using InventarioEscolar.Domain.Entities;
using NSubstitute;

namespace CommonTestUtilities.Repositories.ReportsRepository
{
    public class AssetCanceledMovementsReportGeneratorBuilder
    {
        private readonly IAssetCanceledMovementsReportGenerator _generator;

        public AssetCanceledMovementsReportGeneratorBuilder()
        {
            _generator = Substitute.For<IAssetCanceledMovementsReportGenerator>();
        }
        public AssetCanceledMovementsReportGeneratorBuilder WithGeneratedReport(string schoolName, IList<AssetMovement> movements, byte[] result)
        {
            _generator.Generate(schoolName, movements, Arg.Any<DateTime>()).Returns(result);
            return this;
        }

        public IAssetCanceledMovementsReportGenerator Build() => _generator;
    }
}