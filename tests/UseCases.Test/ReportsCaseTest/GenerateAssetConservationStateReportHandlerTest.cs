using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.ReportsRepository;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetConservationCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;

namespace UseCases.Test.ReportsCaseTest
{
    public class GenerateAssetConservationStateReportHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldGenerateReport_WhenQueryIsValid()
        {
            var school = SchoolBuilder.Build();
            var assets = AssetBuilder.BuildList(1);
            var expectedBytes = new byte[] { 1, 2, 3 };

            var assetRepository = CreateAssetReportReadRepository(assets);
            var schoolRepository = CreateSchoolReadRepository(school);
            var currentUserService = CreateCurrentUserService(true, school.Id);
            var reportGenerator = CreateReportGenerator(school.Name, assets, expectedBytes);

            var handler = CreateUseCase(
                assetRepository,
                reportGenerator,
                currentUserService,
                schoolRepository
            );

            var query = new GenerateAssetConservationStateReportQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldBe(expectedBytes);
            await assetRepository.Received(1).GetAllAssetReport();
            await schoolRepository.Received(1).GetById(school.Id);
            reportGenerator.Received(1).Generate(school.Name, assets, Arg.Any<DateTime>());
        }

        private static GenerateAssetConservationStateReportHandler CreateUseCase(
            IAssetReportReadOnlyRepository assetRepository,
            IAssetConservationStateReportGenerator reportGenerator,
            ICurrentUserService currentUserService,
            ISchoolReadOnlyRepository schoolReadOnlyRepository)
        {
            return new GenerateAssetConservationStateReportHandler(
                assetRepository,
                reportGenerator,
                currentUserService,
                schoolReadOnlyRepository
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

        private static IAssetConservationStateReportGenerator CreateReportGenerator(string schoolName, IList<Asset> assets, byte[] expectedBytes)
        {
            return new AssetConservationStateReportGeneratorBuilder()
                .WithGeneratedReport(schoolName, assets, expectedBytes)
                .Build();
        }
    }
}