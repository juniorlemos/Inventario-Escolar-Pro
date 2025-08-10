using InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase;
using InventarioEscolar.Domain.Entities;
using NSubstitute;

namespace CommonTestUtilities.Repositories.ReportsRepository
{
    public class AssetConservationStateReportGeneratorBuilder
    {
        private readonly IAssetConservationStateReportGenerator reportGenerator;
        public AssetConservationStateReportGeneratorBuilder()
        {
            reportGenerator = Substitute.For<IAssetConservationStateReportGenerator>();
        }
        public AssetConservationStateReportGeneratorBuilder WithGeneratedReport(
            string schoolName, IList<Asset> assets, byte[] expectedBytes)
        {
            reportGenerator
                .Generate(schoolName, assets, Arg.Any<DateTime>())
                .Returns(expectedBytes);
            return this;
        }
        public IAssetConservationStateReportGenerator Build()
        {
            return reportGenerator;
        }
    }
}