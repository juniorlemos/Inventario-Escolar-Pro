using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.ReportsRepository;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;
using Shouldly;

using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;

namespace UseCases.Test.ReportsCaseTest
{

    public class GenerateAssetMovementsReportHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnGeneratedReport_WhenCalledWithValidData()
        {
            var school = SchoolBuilder.Build();
            var movements = AssetMovementBuilder.BuildList(3);
            var expectedBytes = new byte[] { 42, 99 };

            var repository = CreateMovementReportRepository(movements);
            var schoolRepository = CreateSchoolRepository(school);
            var currentUserService = CreateCurrentUserService(true, school.Id);
            var reportGenerator = CreateReportGenerator(school.Name, movements, expectedBytes);

            var handler = CreateHandler(
                repository,
                reportGenerator,
                currentUserService,
                schoolRepository
            );

            var query = new GenerateAssetMovementsReportQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldBe(expectedBytes);
            await repository.Received(1).GetAllAssetMovementsReport();
            var _ = currentUserService.Received(1).SchoolId;
            await schoolRepository.Received(1).GetById(school.Id);
            reportGenerator.Received(1).Generate(school.Name, movements, Arg.Any<DateTime>());
        }

        private static GenerateAssetMovementsReportHandler CreateHandler(
            IAssetMovementReportReadOnlyRepository repository,
            IAssetMovementsReportGenerator reportGenerator,
            ICurrentUserService currentUserService,
            ISchoolReadOnlyRepository schoolRepository)
        {
            return new GenerateAssetMovementsReportHandler(
                repository,
                reportGenerator,
                currentUserService,
                schoolRepository
            );
        }

        private static IAssetMovementReportReadOnlyRepository CreateMovementReportRepository(List<AssetMovement> movements)
        {
            return new AssetMovementReportReadOnlyRepositoryBuilder()
                .WithMovements(movements)
                .Build();
        }

        private static ISchoolReadOnlyRepository CreateSchoolRepository(School school)
        {
            return new SchoolReadOnlyRepositoryBuilder()
                .WithSchoolExist(school.Id, school)
                .Build();
        }

        private static IAssetMovementsReportGenerator CreateReportGenerator(string schoolName, IList<AssetMovement> movements, byte[] expectedBytes)
        {
            return new AssetMovementsReportGeneratorBuilder()
                .WithGeneratedReport(schoolName, movements, expectedBytes)
                .Build();
        }
    }
}