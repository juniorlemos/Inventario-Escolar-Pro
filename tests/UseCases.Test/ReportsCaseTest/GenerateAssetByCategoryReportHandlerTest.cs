using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.ReportsRepository;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByCategoryCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;

namespace UseCases.Test.ReportsCaseTest
{
    public class GenerateAssetByCategoryReportHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnGeneratedReport_WhenCalledWithValidRequest()
        {
            var school = SchoolBuilder.Build();
            var assets = AssetBuilder.BuildList(10);

            var expectedBytes = new byte[] { 5, 10, 15 };

            var assetRepository = CreateAssetReportReadRepository(assets);

            var schoolRepository = CreateSchoolReadRepository(school);

            var currentUserService = CreateCurrentUserService(true, school.Id);

            var reportGenerator = createReportGenerator(school.Name, assets, expectedBytes);

            var handler = createUseCase (
                assetRepository,
                reportGenerator,
                currentUserService,
                schoolRepository
            );

            var query = new GenerateAssetByCategoryReportQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldBe(expectedBytes);
            await assetRepository.Received(1).GetAllAssetReport();
            var _ = currentUserService.Received(1).SchoolId;
            await schoolRepository.Received(1).GetById(school.Id);
            reportGenerator.Received(1).Generate(school.Name, assets, Arg.Any<DateTime>());
        }
        private static GenerateAssetByCategoryReportHandler createUseCase(
            IAssetReportReadOnlyRepository assetRepository,
            IAssetByCategoryReportGenerator reportGenerator,
            ICurrentUserService currentUserService,
            ISchoolReadOnlyRepository schoolReadOnlyRepository)
        {
            return new GenerateAssetByCategoryReportHandler(
                assetRepository,
                reportGenerator,
                currentUserService,
                schoolReadOnlyRepository
            );
        }
        private static IAssetReportReadOnlyRepository CreateAssetReportReadRepository(
            List<Asset> assets)
        {
            var assetRepository = new AssetReportReadOnlyRepositoryBuilder()
                .WithAssets(assets)
                .Build();

            return assetRepository;
        }
        private static ISchoolReadOnlyRepository CreateSchoolReadRepository(
            School school)
        {
            return new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolExist(school.Id, school)
                .Build();
        }
        private static IAssetByCategoryReportGenerator createReportGenerator(string schoolName,IList<Asset> assets, byte[] expectedBytes)
        {
            return new AssetByCategoryReportGeneratorBuilder()
                .WithGeneratedReport(schoolName, assets, expectedBytes)
                .Build();
        }
    }
}