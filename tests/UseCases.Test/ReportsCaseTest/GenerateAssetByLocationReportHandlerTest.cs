using InventarioEscolar.Application.Services.Interfaces;
using InventarioEscolar.Application.UsesCases.ReportsCase.AssetsByLocationCase;
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
    public class GenerateAssetByLocationReportHandlerTest
    {
        [Fact]
        public async Task Handle_ShouldReturnGeneratedReport_WhenCalledWithValidRequest()
        {
            // Arrange
            var assetRepository = Substitute.For<IAssetReportReadOnlyRepository>();
            var reportGenerator = Substitute.For<IAssetByLocationReportGenerator>();
            var currentUserService = Substitute.For<ICurrentUserService>();
            var schoolRepository = Substitute.For<ISchoolReadOnlyRepository>();

            var schoolId = 7;
            var school = new School { Id = schoolId, Name = "Escola Central" };

            var assets = new List<Asset>
            {
                new Asset { Name = "Notebook", PatrimonyCode = 998877 }
            };

            var expectedBytes = new byte[] { 42, 84, 126 };

            assetRepository.GetAllAssetReport().Returns(assets);
            currentUserService.SchoolId.Returns(schoolId);
            schoolRepository.GetById(schoolId).Returns(school);
            reportGenerator.Generate(school.Name, assets, Arg.Any<DateTime>()).Returns(expectedBytes);

            var handler = new GenerateAssetByLocationReportHandler(
                assetRepository,
                reportGenerator,
                currentUserService,
                schoolRepository
            );

            var query = new GenerateAssetByLocationReportQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ShouldBe(expectedBytes);

            await assetRepository.Received(1).GetAllAssetReport();
            var _ = currentUserService.Received(1).SchoolId;
            await schoolRepository.Received(1).GetById(schoolId);
            reportGenerator.Received(1).Generate(school.Name, assets, Arg.Any<DateTime>());
        }
    }
}