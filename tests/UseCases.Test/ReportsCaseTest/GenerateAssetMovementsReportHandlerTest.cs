using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetMovementsCase;
using InventarioEscolar.Domain.Entities;
using InventarioEscolar.Domain.Interfaces.Repositories.Schools;
using InventarioEscolar.Domain.Interfaces.RepositoriesReports;
using NSubstitute;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Test.ReportsCaseTest
{
   
        public class GenerateAssetMovementsReportHandlerTest
        {
            [Fact]
            public async Task Handle_ShouldGenerateReport_WhenDataIsValid()
            {
                // Arrange
                var repository = Substitute.For<IAssetMovementReportReadOnlyRepository>();
                var reportGenerator = Substitute.For<IAssetMovementsReportGenerator>();
                var currentUserService = Substitute.For<ICurrentUserService>();
                var schoolRepository = Substitute.For<ISchoolReadOnlyRepository>();

                var schoolId = 2;
                var school = new School { Id = schoolId, Name = "Escola Teste" };

                var movements = new List<AssetMovement>
            {
                new AssetMovement
    {
        Id = 1,
        AssetId = 100,
        Asset = new Asset { Id = 100, Name = "Computador" },
        FromRoomId = 1,
        FromRoom = new RoomLocation { Id = 1, Name = "Sala 1" },
        ToRoomId = 2,
        ToRoom = new RoomLocation { Id = 2, Name = "Sala 2" },
        MovedAt = DateTime.UtcNow,
        Responsible = "João",
        IsCanceled = false
    }
            };

                var expectedBytes = new byte[] { 0x1, 0x2 };

                repository.GetAllAssetMovementsReport().Returns(movements);
                currentUserService.SchoolId.Returns(schoolId);
                schoolRepository.GetById(schoolId).Returns(school);
                reportGenerator.Generate(school.Name, movements, Arg.Any<DateTime>()).Returns(expectedBytes);

                var handler = new GenerateAssetMovementsReportHandler(
                    repository,
                    reportGenerator,
                    currentUserService,
                    schoolRepository
                );

                var query = new GenerateAssetMovementsReportQuery();

                // Act
                var result = await handler.Handle(query, CancellationToken.None);

                // Assert
                result.ShouldBe(expectedBytes);
                await repository.Received(1).GetAllAssetMovementsReport();
                _ = currentUserService.Received(1).SchoolId;
                await schoolRepository.Received(1).GetById(schoolId);
                reportGenerator.Received(1).Generate(school.Name, movements, Arg.Any<DateTime>());
            }
        }
}