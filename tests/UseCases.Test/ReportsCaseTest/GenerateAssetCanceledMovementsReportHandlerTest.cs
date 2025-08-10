using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories.ReportsRepository;
using CommonTestUtilities.Repositories.SchoolRepository;
using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;
using Shouldly;
using static CommonTestUtilities.Helpers.CurrentUserServiceTestHelper;

namespace UseCases.Test.ReportsCaseTest
{   
        public class GenerateAssetCanceledMovementsReportHandlerTest
        {
            [Fact]
            public async Task Handle_ShouldReturnGeneratedReport_WhenDataIsValid()
            {
                var school = SchoolBuilder.Build();
                var movements = AssetMovementBuilder.BuildList(5);
                var expectedBytes = new byte[] { 1, 2, 3 };

                var repository = CreateMovementReportRepository(movements);
                var schoolRepository = CreateSchoolReadRepository(school);
                var currentUserService = CreateCurrentUserService(true, school.Id);
                var reportGenerator = CreateReportGenerator(school.Name, movements, expectedBytes);

                var handler = CreateUseCase(repository, reportGenerator, currentUserService, schoolRepository);

                var query = new GenerateAssetCanceledMovementsReportQuery();

                var result = await handler.Handle(query, CancellationToken.None);

                result.ShouldBe(expectedBytes);
                await repository.Received(1).GetAllAssetCanceledMovementsReport();
                var _ = currentUserService.Received(1).SchoolId;
                await schoolRepository.Received(1).GetById(school.Id);
                reportGenerator.Received(1).Generate(school.Name, movements, Arg.Any<DateTime>());
            }

            [Fact]
            public async Task Handle_ShouldCallGetAllAssetCanceledMovementsReport_Once()
            {
                var school = SchoolBuilder.Build();
                var repository = CreateMovementReportRepository(new List<AssetMovement>());
                var schoolRepository = CreateSchoolReadRepository(school);
                var currentUserService = CreateCurrentUserService(true, school.Id);
                var reportGenerator = CreateReportGenerator(school.Name, new List<AssetMovement>(), Array.Empty<byte>());

                var handler = CreateUseCase(repository, reportGenerator, currentUserService, schoolRepository);

                var query = new GenerateAssetCanceledMovementsReportQuery();

                await handler.Handle(query, CancellationToken.None);

                await repository.Received(1).GetAllAssetCanceledMovementsReport();
            }

            [Fact]
            public async Task Handle_ShouldCallGetById_WithCorrectSchoolId()
            {
                var school = SchoolBuilder.Build();
                var repository = CreateMovementReportRepository(new List<AssetMovement>());
                var schoolRepository = CreateSchoolReadRepository(school);
                var currentUserService = CreateCurrentUserService(true, school.Id);
                var reportGenerator = CreateReportGenerator(school.Name, new List<AssetMovement>(), Array.Empty<byte>());

                var handler = CreateUseCase(repository, reportGenerator, currentUserService, schoolRepository);

                var query = new GenerateAssetCanceledMovementsReportQuery();

                await handler.Handle(query, CancellationToken.None);

                await schoolRepository.Received(1).GetById(school.Id);
            }

            private static GenerateAssetCanceledMovementsReportHandler CreateUseCase(
                IAssetMovementReportReadOnlyRepository repository,
                IAssetCanceledMovementsReportGenerator reportGenerator,
                ICurrentUserService currentUserService,
                ISchoolReadOnlyRepository schoolRepository)
            {
                return new GenerateAssetCanceledMovementsReportHandler(
                    repository,
                    reportGenerator,
                    currentUserService,
                    schoolRepository
                );
            }

        private static IAssetMovementReportReadOnlyRepository CreateMovementReportRepository(List<AssetMovement> movements)
        {
            return new AssetMovementReportReadOnlyRepositoryBuilder()
                .WithCanceledMovements(movements)
                .Build();
        }

        private static ISchoolReadOnlyRepository CreateSchoolReadRepository(School school)
            {
                return new SchoolReadOnlyRepositoryBuilder()
                    .WithSchoolExist(school.Id, school)
                    .Build();
            }

            private static IAssetCanceledMovementsReportGenerator CreateReportGenerator(string schoolName, IList<AssetMovement> movements, byte[] expectedBytes)
            {
                return new AssetCanceledMovementsReportGeneratorBuilder()
                    .WithGeneratedReport(schoolName, movements, expectedBytes)
                    .Build();
            }
        }
    }