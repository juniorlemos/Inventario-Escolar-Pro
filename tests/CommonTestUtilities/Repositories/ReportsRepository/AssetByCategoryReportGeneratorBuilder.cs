using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase;
using InventarioEscolar.Domain.Entities;
using NSubstitute;

namespace CommonTestUtilities.Repositories.ReportsRepository
{
    public class AssetByCategoryReportGeneratorBuilder
    {
        private readonly IAssetByCategoryReportGenerator generator;

        public AssetByCategoryReportGeneratorBuilder()
        {
            generator = Substitute.For<IAssetByCategoryReportGenerator>();
        }
        public AssetByCategoryReportGeneratorBuilder WithGeneratedReport(string schoolName, IList<Asset> assets, byte[] report)
        {
            generator.Generate(schoolName, assets, Arg.Any<DateTime>()).Returns(report);
            return this;
        }
        public IAssetByCategoryReportGenerator Build() => generator;
    }
}