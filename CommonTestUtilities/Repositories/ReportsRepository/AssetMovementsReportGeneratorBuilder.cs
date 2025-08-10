using InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase;
using InventarioEscolar.Domain.Entities;
using NSubstitute;

namespace CommonTestUtilities.Repositories.ReportsRepository
{
    public class AssetMovementsReportGeneratorBuilder
    {
        private readonly IAssetMovementsReportGenerator _generator;

        public AssetMovementsReportGeneratorBuilder()
        {
            _generator = Substitute.For<IAssetMovementsReportGenerator>();
        }
        public AssetMovementsReportGeneratorBuilder WithGeneratedReport(string schoolName, IList<AssetMovement> movements, byte[] result)
        {
            _generator.Generate(schoolName, movements, Arg.Any<DateTime>()).Returns(result);
            return this;
        }
        public IAssetMovementsReportGenerator Build() => _generator;
    }
}