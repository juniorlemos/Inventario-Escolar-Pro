using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.InventoryCase;
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
    public class GenerateInventoryReportQueryHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnGeneratedReport_WhenCalledWithValidRequest()
        {
            // Arrange
            var repository = Substitute.For<IAssetReportReadOnlyRepository>();
            var reportGenerator = Substitute.For<IInventoryReportGenerator>();
            var currentUserService = Substitute.For<ICurrentUserService>();
            var schoolRepository = Substitute.For<ISchoolReadOnlyRepository>();

            var schoolId = 5;
            var school = new School { Id = schoolId, Name = "Escola Modelo" };

            var assets = new List<Asset>
            {
                new Asset { Name = "Projetor", PatrimonyCode = 55854 }
            };

            var expectedBytes = new byte[] { 10, 20, 30 };

            repository.GetAllAssetReport().Returns(assets);
            currentUserService.SchoolId.Returns(schoolId);
            schoolRepository.GetById(schoolId).Returns(school);
            reportGenerator.Generate(school.Name, assets, Arg.Any<DateTime>()).Returns(expectedBytes);

            var handler = new GenerateInventoryReportQueryHandler(
                repository,
                reportGenerator,
                currentUserService,
                schoolRepository
            );

            var query = new GenerateInventoryReportQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBe(expectedBytes);

            await repository.Received(1).GetAllAssetReport();
            var _ = currentUserService.Received(1).SchoolId;
            await schoolRepository.Received(1).GetById(schoolId);
            reportGenerator.Received(1).Generate(school.Name, assets, Arg.Any<DateTime>());
        }
    }
}
