using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.ReportsRepository;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;

namespace UseCases.Test.ReportsCaseTest
{
    public class GenerateInventoryReportQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnGeneratedReport_WhenCalledWithValidRequest()
        {
            var school = SchoolBuilder.Build();
            var assets = AssetBuilder.BuildList(10);
            var expectedBytes = new byte[] { 10, 20, 30 };

            var assetRepository = CreateAssetReportReadRepository(assets);
            var schoolRepository = CreateSchoolReadRepository(school);
            var currentUserService = CreateCurrentUserService(true, school.Id);
            var reportGenerator = CreateReportGenerator(school.Name, assets, expectedBytes);

            var handler = CreateHandler(
                assetRepository,
                reportGenerator,
                currentUserService,
                schoolRepository
            );

            var query = new GenerateInventoryReportQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldBe(expectedBytes);
            await assetRepository.Received(1).GetAllAssetReport();
            var _ = currentUserService.Received(1).SchoolId;
            await schoolRepository.Received(1).GetById(school.Id);
            reportGenerator.Received(1).Generate(school.Name, assets, Arg.Any<DateTime>());
        }

        private static GenerateInventoryReportHandler CreateHandler(
            IAssetReportReadOnlyRepository assetRepository,
            IInventoryReportGenerator reportGenerator,
            ICurrentUserService currentUserService,
            ISchoolReadOnlyRepository schoolRepository)
        {
            return new GenerateInventoryReportHandler(
                assetRepository,
                reportGenerator,
                currentUserService,
                schoolRepository
            );
        }

        private static IAssetReportReadOnlyRepository CreateAssetReportReadRepository(List<Asset> assets)
        {
            return new AssetReportReadOnlyRepositoryBuilder()
                .WithAssets(assets)
                .Build();
        }

        private static ISchoolReadOnlyRepository CreateSchoolReadRepository(School school)
        {
            return new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolExist(school.Id, school)
                .Build();
        }

        private static IInventoryReportGenerator CreateReportGenerator(string schoolName, IList<Asset> assets, byte[] expectedBytes)
        {
            return new InventoryReportGeneratorBuilder()
                .WithGeneratedReport(schoolName, assets, expectedBytes)
                .Build();
        }
    }
}