using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetCanceledMovementsCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.ReportsCaseTest
{
    public class GenerateAssetCanceledMovementsReportHandlerTest
    {
        public class GenerateAssetCanceledMovementsReportHandlerTests
        {
            [Fact]
            public async Task Handle_ShouldReturnGeneratedReport_WhenDataIsValid()
            {
                // Arrange
                var repository = Substitute.For<IAssetMovementReportReadOnlyRepository>();
                var reportGenerator = Substitute.For<IAssetCanceledMovementsReportGenerator>();
                var currentUserService = Substitute.For<ICurrentUserService>();
                var schoolRepository = Substitute.For<ISchoolReadOnlyRepository>();

                var schoolId = 1L;
                var schoolName = "Escola Teste";
                var movements = new List<AssetMovement>();
                var expectedReport = new byte[] { 1, 2, 3 };

                currentUserService.SchoolId.Returns(schoolId);
                schoolRepository.GetById(schoolId).Returns(new School { Id = schoolId, Name = schoolName });
                repository.GetAllAssetCanceledMovementsReport().Returns(movements);
                reportGenerator.Generate(Arg.Any<string>(), movements, Arg.Any<DateTime>()).Returns(expectedReport);

                var handler = new GenerateAssetCanceledMovementsReportHandler(
                    repository,
                    reportGenerator,
                    currentUserService,
                    schoolRepository
                );

                var request = new GenerateAssetCanceledMovementsReportQuery();

                // Act
                var result = await handler.Handle(request, CancellationToken.None);

                // Assert
                Assert.Equal(expectedReport, result);
            }

            [Fact]
            public async Task Handle_ShouldCallGetAllAssetCanceledMovementsReport_Once()
            {
                // Arrange
                var repository = Substitute.For<IAssetMovementReportReadOnlyRepository>();
                var reportGenerator = Substitute.For<IAssetCanceledMovementsReportGenerator>();
                var currentUserService = Substitute.For<ICurrentUserService>();
                var schoolRepository = Substitute.For<ISchoolReadOnlyRepository>();

                currentUserService.SchoolId.Returns(1);
                schoolRepository.GetById(Arg.Any<long>()).Returns(new School { Id = 1, Name = "Teste" });
                repository.GetAllAssetCanceledMovementsReport().Returns(new List<AssetMovement>());
                reportGenerator.Generate(Arg.Any<string>(), Arg.Any<List<AssetMovement>>(), Arg.Any<DateTime>()).Returns(Array.Empty<byte>());

                var handler = new GenerateAssetCanceledMovementsReportHandler(
                    repository,
                    reportGenerator,
                    currentUserService,
                    schoolRepository
                );

                var request = new GenerateAssetCanceledMovementsReportQuery();

                // Act
                await handler.Handle(request, CancellationToken.None);

                // Assert
                await repository.Received(1).GetAllAssetCanceledMovementsReport();
            }

            [Fact]
            public async Task Handle_ShouldCallGetById_WithCorrectSchoolId()
            {
                // Arrange
                var repository = Substitute.For<IAssetMovementReportReadOnlyRepository>();
                var reportGenerator = Substitute.For<IAssetCanceledMovementsReportGenerator>();
                var currentUserService = Substitute.For<ICurrentUserService>();
                var schoolRepository = Substitute.For<ISchoolReadOnlyRepository>();

                var schoolId = 42L;
                currentUserService.SchoolId.Returns(schoolId);
                schoolRepository.GetById(schoolId).Returns(new School { Id = schoolId, Name = "Escola XYZ" });
                repository.GetAllAssetCanceledMovementsReport().Returns(new List<AssetMovement>());
                reportGenerator.Generate(Arg.Any<string>(), Arg.Any<List<AssetMovement>>(), Arg.Any<DateTime>()).Returns(Array.Empty<byte>());

                var handler = new GenerateAssetCanceledMovementsReportHandler(
                    repository,
                    reportGenerator,
                    currentUserService,
                    schoolRepository
                );

                var request = new GenerateAssetCanceledMovementsReportQuery();

                // Act
                await handler.Handle(request, CancellationToken.None);

                // Assert
                await schoolRepository.Received(1).GetById(schoolId);
            }
        }
    }
}
