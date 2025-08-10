using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase;
using InventarioEscolar.Domain.Entities;
using NSubstitute;

namespace CommonTestUtilities.Repositories.ReportsRepository
{
    public class AssetByLocationReportGeneratorBuilder
    {
        private readonly IAssetByLocationReportGenerator generator;
        public AssetByLocationReportGeneratorBuilder()
        {
            generator = Substitute.For<IAssetByLocationReportGenerator>();
        }
        public AssetByLocationReportGeneratorBuilder WithGeneratedReport(string schoolName, IList<Asset> assets, byte[] report)
        {
            generator.Generate(schoolName, assets, Arg.Any<DateTime>()).Returns(report);
            return this;
        }

        public IAssetByLocationReportGenerator Build() => generator;
    }
}