using InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase;
using InventarioEscolar.Domain.Entities;
using NSubstitute;

namespace CommonTestUtilities.Repositories.ReportsRepository
{
    public class InventoryReportGeneratorBuilder
    {
        private readonly IInventoryReportGenerator generator;

        public InventoryReportGeneratorBuilder()
        {
            generator = Substitute.For<IInventoryReportGenerator>();
        }
        public InventoryReportGeneratorBuilder WithGeneratedReport(string schoolName, IList<Asset> assets, byte[] report)
        {
            generator.Generate(schoolName, assets, Arg.Any<DateTime>()).Returns(report);
            return this;
        }
        public IInventoryReportGenerator Build() => generator;
    }
}